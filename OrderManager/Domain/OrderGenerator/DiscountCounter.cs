using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using System.Collections.Generic;
using System.Linq;

namespace OrderManager.Domain
{
    public class DiscountCounter
    {
        private Dictionary<CounterpartysStock, int> dictionary;
        private ICounterpartysStockService counterpartysStockService;
        private IStockService stockService;

        public DiscountCounter(Dictionary<CounterpartysStock, int> dictionary, 
            ICounterpartysStockService counterpartysStockService,
            IStockService stockService)
        {
            this.dictionary = dictionary;
            this.counterpartysStockService = counterpartysStockService;
            this.stockService = stockService;
        }

        public DiscountCounter(Dictionary<Stock, int> dictionary,
            ICounterpartysStockService counterpartysStockService,
            IStockService stockService)
        {
            this.dictionary = new Dictionary<CounterpartysStock, int>();
            this.counterpartysStockService = counterpartysStockService;
            this.stockService = stockService;
            foreach (var oneStock in dictionary)
                foreach (var counterpartysStock in stockService.GetStocksCounterpartysStock(oneStock.Key))
                    (this.dictionary).Add(counterpartysStock, oneStock.Value);
        }

        public List<Tranche> BestChosenDiscounts()
        {
            if (dictionary.Count == 0) return new List<Tranche>();
            List<CounterpartysStock> bestStockWithoutDiscounts = lowestPricesWithoutDiscount(dictionary);
            HashSet<PercentageDiscount> possibleDiscounts = new HashSet<PercentageDiscount>(
                dictionary.Keys.Select(
                counterpartysStock => counterpartysStockService.GetValidDiscounts(counterpartysStock))
                .SelectMany(i => i));
            foreach (var discount in possibleDiscounts)
                adjustDiscountToDemand(discount);
            possibleDiscounts.RemoveWhere(discount => !(discountCanBeUsed(discount)));
            List<PercentageDiscount> discountsInOrders = new List<PercentageDiscount>();
            if (possibleDiscounts.Count != 0)
            {
                List<PercentageDiscount> allPossibleConfigurationsOfDiscounts = new List<PercentageDiscount>();
                foreach (var discount in possibleDiscounts)
                    allPossibleConfigurationsOfDiscounts.AddRange(allPossibleCombinationsOfDiscount(discount));
                List<List<PercentageDiscount>> allCombosOfDiscounts = getAllCombos(allPossibleConfigurationsOfDiscounts);
                allCombosOfDiscounts.RemoveAll(listOfDiscounts => !(discountsAreDisjunctive(listOfDiscounts)));
                Dictionary<List<PercentageDiscount>, double> profits = getProfits(
                   allCombosOfDiscounts, bestStockWithoutDiscounts);
                discountsInOrders = getProfits(allCombosOfDiscounts, bestStockWithoutDiscounts)
                    .OrderByDescending(tuple => tuple.Value).First().Key;
            }
            return makeTranches(discountsInOrders, bestStockWithoutDiscounts);
        }

        private bool discountsAreDisjunctive(List<PercentageDiscount> discounts)
        {
            var stockWithRepetitions = discounts.Select(disc =>
            disc.CounterpartysStock).SelectMany(i => i).Select(stock => stock.Stock);
            return stockWithRepetitions.SequenceEqual(new HashSet<Stock>(stockWithRepetitions));
        }
        
        private List<CounterpartysStock> lowestPricesWithoutDiscount(Dictionary<CounterpartysStock, int> dictionary)
        {
            List<CounterpartysStock> result = new List<CounterpartysStock>();
            foreach (var stock in
                new HashSet<Stock>(dictionary.Keys.Select(counterpartysStock => counterpartysStock.Stock)))
                result.Add(stockService.GetStocksCounterpartysStock(stock).Intersect(dictionary.Keys).OrderBy(
                    counterpartysStock => counterpartysStock.PriceNetto).First());
            return result;
        }

        private void adjustDiscountToDemand(PercentageDiscount discount)
        {
            discount.CounterpartysStock = new List<CounterpartysStock>
                (discount.CounterpartysStock.Intersect(dictionary.Keys));
        }

        private bool discountCanBeUsed(PercentageDiscount discount)
        {
            double valueOfOrder = 0;
            foreach (var element in discount.CounterpartysStock)
                valueOfOrder += dictionary[element] * element.PriceNetto;
            return valueOfOrder >= discount.SumNetto;
        }
        
        private Dictionary<List<PercentageDiscount>, double> getProfits(List<List<PercentageDiscount>> combinationsOfDiscounts, List<CounterpartysStock> bestStockWithoutDiscounts)
        {
            Dictionary<List<PercentageDiscount>, double> result = new Dictionary<List<PercentageDiscount>, double>();
            foreach (var combination in combinationsOfDiscounts)
                result.Add(combination, getProfit(combination, bestStockWithoutDiscounts));
            return result;
        }
        
        private double getProfit(List<PercentageDiscount> discountsInOneSetOfOrders, List<CounterpartysStock> bestStockWithoutDiscounts) 
        {
            var stockWithDiscount = new HashSet<Stock>(
                discountsInOneSetOfOrders.Select(discount => discount.CounterpartysStock).SelectMany(i => i)
                .Select(counterpartysStock => counterpartysStock.Stock));

            double profit = 0;

            foreach (var discount in discountsInOneSetOfOrders)
                foreach (var counterpartysStock in discount.CounterpartysStock)
                    profit += (bestStockWithoutDiscounts.FindLast(bestStock => bestStock.Stock.Equals(counterpartysStock.Stock)).PriceNetto
                        - counterpartysStock.PriceNetto * (1 - discount.Amount)) 
                        * dictionary[counterpartysStock];
            return profit;
        }
        
        private List<PercentageDiscount> allPossibleCombinationsOfDiscount(PercentageDiscount discount)
        {
            List<CounterpartysStock> counterpartysStockInDiscount = discount.CounterpartysStock;
            List<PercentageDiscount> result = new List<PercentageDiscount>();
            PercentageDiscount currentDiscount;
            List<List<CounterpartysStock>> allCombos = getAllCombos(counterpartysStockInDiscount);
            foreach (var combo in allCombos)
                if (discountCanBeUsed(currentDiscount = new PercentageDiscount(discount, combo)))
                    result.Add(currentDiscount);
            return result;
        }

        private List<Tranche> makeTranches(List<PercentageDiscount> discountsInOrders,
            List<CounterpartysStock> stockWtithLowestPricesWithoutDiscount)
        {
            List<Tranche> result = new List<Tranche>();
            foreach (var discount in discountsInOrders)
                foreach (var counterpartysStock in discount.CounterpartysStock)
                    result.Add(new Tranche(null, dictionary[counterpartysStock], counterpartysStock,
                        (new PercentageDiscount[] { discount }).ToList()));

            CounterpartysStock currentCounterpartysStock;
            foreach (var stock in new HashSet<Stock>(
                dictionary.Keys.Select(counterpartysStock => counterpartysStock.Stock))
                .Except(
                    discountsInOrders.Select(discount => discount.CounterpartysStock).SelectMany(i => i).
                    Select(counterpartysStock => counterpartysStock.Stock)))
                result.Add(new Tranche(null, currentCounterpartysStock = stockWtithLowestPricesWithoutDiscount.
                    Where(counterpartysStock => counterpartysStock.Stock.Equals(stock)).First(),
                    dictionary[currentCounterpartysStock]));

            return result;
        }

        private List<List<T>> getAllCombos<T>(List<T> list)
        {
            List<List<T>> result = new List<List<T>>();
            result.Add(new List<T>());
            result.Last().Add(list[0]);
            if (list.Count == 1)
                return result;
            List<List<T>> tailCombos = getAllCombos(list.Skip(1).ToList());
            tailCombos.ForEach(combo =>
            {
                result.Add(new List<T>(combo));
                combo.Add(list[0]);
                result.Add(new List<T>(combo));
            });
            return result;
        }

    }
}
