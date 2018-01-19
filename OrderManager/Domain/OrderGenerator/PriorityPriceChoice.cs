using System.Collections.Generic;
using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;

namespace OrderManager.Domain.OrderGenerator
{
    class PriorityPriceChoice : IOffersChoice
    {
        Dictionary<Stock, int> stockToOrder;
        ICounterpartysStockService counterpartysStockService;
        IStockService stockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityPriceChoice"/> class.
        /// </summary>
        /// <param name="stockToOrder">The stock to order.</param>
        /// <param name="counterpartysStockService">The counterpartys stock service.</param>
        /// <param name="stockService">The stock service.</param>
        public PriorityPriceChoice(Dictionary<Stock, int> stockToOrder,
            ICounterpartysStockService counterpartysStockService, IStockService stockService)
        {
            this.stockToOrder = stockToOrder;
            this.counterpartysStockService = counterpartysStockService;
            this.stockService = stockService;
        }

        /// <summary>
        /// Gets a list of tranches with properties representing the best chosen offers, decided by the lowest price.
        /// </summary>
        /// <returns>Returns a list of tranche entities with properties representing the best offers chosen by price</returns>
        public List<Tranche> BestChosenOffers()
        {
            return (new DiscountCounter(stockToOrder, counterpartysStockService, stockService)).BestChosenDiscounts();
        }
    }
}
