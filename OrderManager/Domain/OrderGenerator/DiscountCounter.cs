using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManager.Domain.OrderGenerator
{
    public class DiscountCounter
    {
        private Dictionary<CounterpartysStock, int> dictionary;
        private ICounterpartysStockService counterpartysStockService;
        private IStockService stockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscountCounter"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="counterpartysStockService">The counterpartys stock service.</param>
        /// <param name="stockService">The stock service.</param>
        public DiscountCounter(Dictionary<CounterpartysStock, int> dictionary, 
            ICounterpartysStockService counterpartysStockService,
            IStockService stockService)
        {
            this.dictionary = dictionary;
            this.counterpartysStockService = counterpartysStockService;
            this.stockService = stockService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscountCounter"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="counterpartysStockService">The counterpartys stock service.</param>
        /// <param name="stockService">The stock service.</param>
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

        /// <summary>
        /// Gets the list of tranches with the best possible discounts.
        /// </summary>
        /// <returns>Returns the list of tranches with the best possible discounts.</returns>
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

        /// <summary>
        /// Boolean representing if the discounts contained in a given list are a disjunctive set.
        /// </summary>
        /// <param name="discounts">The discounts.</param>
        /// <returns>Returns a Boolean representing if the discounts contained in a given list are a disjunctive set.</returns>
        private bool discountsAreDisjunctive(List<PercentageDiscount> discounts)
        {
            var stockWithRepetitions = discounts.Select(disc =>
            disc.CounterpartysStock).SelectMany(i => i).Select(stock => stock.Stock);
            return stockWithRepetitions.SequenceEqual(new HashSet<Stock>(stockWithRepetitions));
        }

        /// <summary>
        /// Gets the list of the counterparty stock that has the lowest price, when not counting discounts.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>Returns the list of the counterparty stock with the lowest base price.</returns>
        private List<CounterpartysStock> lowestPricesWithoutDiscount(Dictionary<CounterpartysStock, int> dictionary)
        {
            List<CounterpartysStock> result = new List<CounterpartysStock>();
            foreach (var stock in
                new HashSet<Stock>(dictionary.Keys.Select(counterpartysStock => counterpartysStock.Stock)))
                result.Add(stockService.GetStocksCounterpartysStock(stock).Intersect(dictionary.Keys).OrderBy(
                    counterpartysStock => counterpartysStock.PriceNetto).First());
            return result;
        }

        /// <summary>
        /// Adjusts the discount to demand.
        /// </summary>
        /// <param name="discount">The discount.</param>
        private void adjustDiscountToDemand(PercentageDiscount discount)
        {
            discount.CounterpartysStock = new List<CounterpartysStock>
                (discount.CounterpartysStock.Intersect(dictionary.Keys));
        }

        /// <summary>
        /// Tells if a given discounts can be used.
        /// </summary>
        /// <param name="discount">The discount.</param>
        /// <returns>Returns a boolean representing if the given discount can be used.</returns>
        private bool discountCanBeUsed(PercentageDiscount discount)
        {
            double valueOfOrder = 0;
            foreach (var element in discount.CounterpartysStock)
                valueOfOrder += dictionary[element] * element.PriceNetto;
            return valueOfOrder >= discount.SumNetto;
        }

        /// <summary>
        /// Gets the profits for a calculated combination of discounts and a list of counterparty stock with lowest base price.
        /// </summary>
        /// <param name="combinationsOfDiscounts">The combinations of discounts.</param>
        /// <param name="bestStockWithoutDiscounts">The best stock without discounts.</param>
        /// <returns>Returns the dictionary of sets of discounts and calculated profits.</returns>
        private Dictionary<List<PercentageDiscount>, double> getProfits(List<List<PercentageDiscount>> combinationsOfDiscounts, List<CounterpartysStock> bestStockWithoutDiscounts)
        {
            Dictionary<List<PercentageDiscount>, double> result = new Dictionary<List<PercentageDiscount>, double>();
            foreach (var combination in combinationsOfDiscounts)
                result.Add(combination, getProfit(combination, bestStockWithoutDiscounts));
            return result;
        }

        /// <summary>
        /// Gets the profits for a given set of discounts and and a list of counterparty stock with lowest base price.
        /// </summary>
        /// <param name="discountsInOneSetOfOrders">The discounts in one set of orders.</param>
        /// <param name="bestStockWithoutDiscounts">The best stock without discounts.</param>
        /// <returns>Returns a double value representing profit.</returns>
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

        /// <summary>
        /// Gets all possible combinations of discounts for a given discount.
        /// </summary>
        /// <param name="discount">The discount.</param>
        /// <returns>Returns a list of discounts representing all possible combinations of discounts for a given discount.</returns>
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

        /// <summary>
        /// Gets tranches with given discounts and stock
        /// </summary>
        /// <param name="discountsInOrders">The discounts in orders.</param>
        /// <param name="stockWtithLowestPricesWithoutDiscount">The stock wtith lowest prices without discount.</param>
        /// <returns>Returns a list of tranche entities with given discounts and stock</returns>
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

        /// <summary>
        /// Generic method for getting all the combinations of list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns>Returns a parametrized list of lists representing all the combinations of list.</returns>
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
