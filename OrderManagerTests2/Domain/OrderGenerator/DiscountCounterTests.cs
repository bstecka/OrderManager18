using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManager.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OrderManager.Domain.Service;
using OrderManager.Domain.Entity;

namespace OrderManager.Domain.Tests
{
    /*
    [TestClass()]
    public class DiscountCounterTests
    {
        private Mock<IStockService> mockStockService;
        private Mock<ICounterpartysStockService> mockCounterpartysStockService;
        private Dictionary<Entity.Stock, int> stockToOrder;
        private Dictionary<Mock<CounterpartysStock>, int> counterpartysStock;

        public DiscountCounterTests()
        {
            mockCounterpartysStockService = new Mock<ICounterpartysStockService>();
            mockStockService = new Mock<IStockService>();
        }
        
        [TestMethod()]
        public void BestChosenDiscountsTest()
        {
            //There's no stock to order.
            stockToOrder = new Dictionary<Entity.Stock, int>();

            DiscountCounter discountCounter = new DiscountCounter(stockToOrder, mockCounterpartysStockService.Object, mockStockService.Object);
            var generatedTranches = discountCounter.BestChosenDiscounts();

            Assert.IsTrue(generatedTranches != null);
            Assert.IsTrue(generatedTranches.Count() == 0);
            //mockCounterpartysStockService.Verify(m => m.GetValidDiscounts(It.IsAny<CounterpartysStock>()), Times.Exactly(0));
        }


        [TestMethod()]
        public void BestChosenDiscountsTest1()
        {
            //There's no stock to order.
            counterpartysStock = new Dictionary<Mock<CounterpartysStock>, int>();
            counterpartysStock.Add(
                new Mock<CounterpartysStock>(1, mockStock(1).Object,
                    new Counterparty(1, 1, "K1", 5), 10),
                    10);
            DiscountCounter discountCounter = new DiscountCounter(
                counterpartysStock.ToDictionary(m => m.Key.Object, m => m.Value), 
                mockCounterpartysStockService.Object, mockStockService.Object);
            var generatedTranches = discountCounter.BestChosenDiscounts();

            Assert.IsTrue(generatedTranches != null);
            Assert.IsTrue(generatedTranches.Count() == 0);
            //mockCounterpartysStockService.Verify(m => m.GetValidDiscounts(It.IsAny<CounterpartysStock>()), Times.Exactly(0));
        }

        private List<Mock<Stock>> getMockedStockList(int count)
        {
            //        public Stock(int id, int maxInStockRoom, int minInStockRoom, 
            //int numberOfImtems, int weightOfItem, int numOfItems, 
            //int VAT, string code, string name, Category category)

            List<Mock<Stock>> stockList = new List<Mock<Stock>>();
            Category category1 = new Category(1, "Kategoria1");
            Category category2 = new Category(1, "Kategoria2");
            stockList.Add(new Mock<Stock>(1, 40, 0, 0, 1, 10, 23, "00001", "T1", category1));
            stockList.Add(new Mock<Stock>(2, 40, 0, 0, 1, 10, 23, "00002", "21", category1));
            stockList.Add(new Mock<Stock>(3, 40, 0, 0, 1, 10, 23, "00003", "T3", category1));
            stockList.Add(new Mock<Stock>(4, 40, 0, 0, 1, 10, 23, "00004", "T4", category1));
            stockList.Add(new Mock<Stock>(5, 40, 0, 0, 1, 10, 23, "00005", "T5", category1));
            stockList.Add(new Mock<Stock>(6, 40, 0, 0, 1, 10, 23, "00006", "T6", category2));
            stockList.Add(new Mock<Stock>(7, 40, 0, 0, 1, 10, 23, "00007", "T7", category2));

            return stockList.GetRange(0, count - 1);
        }

        private Mock<Stock> mockStock(int id)
        {

            var mockThing = new Mock<Stock>(id, 40, 0, 0, 1, 10, 23, "00001", "T1", 
                new Category(1, "Kat1"));

            mockThing.Setup(t => t.Id).Returns(id);
            mockThing.Setup(t => t.Equals(It.IsAny<object>()))
                .Returns<object>(t => (t as Stock)?.Id == id);

            return mockThing;
      }
        
    }*/
}