using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace OrderManager.Domain.Entity.Tests
{
    [TestClass()]
    public class TrancheTests
    {
        Mock<Stock> stockMock;
        Mock<Counterparty> counterpartyMock;
        Mock<CounterpartysStock> counterpartysStockMock;
        Random random;

        public TrancheTests()
        {
            stockMock = new Mock<Stock>(1, 1, 1, 1, 1, 1, 23, "Code", "TestStock", null, false);
            counterpartyMock = new Mock<Counterparty>(1, 11111111111, "TestCounterparty", 1);
            counterpartysStockMock = new Mock<CounterpartysStock>(1, stockMock.Object, counterpartyMock.Object, 1);
            random = new Random();
        }

        [TestMethod()]
        public void TrancheTests1()
        {
            int numberOfItems = 10;
            setExemplaryCounterpartysStock(2);

            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            
            Assert.AreEqual(tranche.Id, 1);
            Assert.AreEqual(tranche.Stock, counterpartysStockMock.Object);
            Assert.AreEqual(tranche.NumberOfItems, numberOfItems);
        }

        [TestMethod()]
        public void TrancheTests2()
        {
            int numberOfItems = 0;
            setExemplaryCounterpartysStock(2);
            Assert.ThrowsException<ArgumentException>(() => new Tranche(1, counterpartysStockMock.Object, numberOfItems));
        }

        [TestMethod()]
        public void TrancheTests3()
        {
            int numberOfItems = -100;
            setExemplaryCounterpartysStock(2);
            Assert.ThrowsException<ArgumentException>(() => new Tranche(1, counterpartysStockMock.Object, numberOfItems));
        }

        [TestMethod()]
        public void TrancheTests4()
        {
            int numberOfItems = 10;
            setExemplaryCounterpartysStock(2);
            Assert.ThrowsException<ArgumentException>(() => new Tranche(1, null, numberOfItems));
        }

        [TestMethod()]
        public void TrancheNumberOfItemsTest()
        {
            int numberOfItems = 10;
            setExemplaryCounterpartysStock(2);
            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            numberOfItems = 100;
            tranche.NumberOfItems = numberOfItems;
            Assert.AreEqual(tranche.NumberOfItems, numberOfItems);
        }

        [TestMethod()]
        public void TrancheNumberOfItemsTest1()
        {
            int numberOfItems = 10;
            setExemplaryCounterpartysStock(2);
            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            Assert.ThrowsException<ArgumentException>(() => tranche.NumberOfItems = 0);
        }

        [TestMethod()]
        public void TrancheNumberOfItemsTest2()
        {
            int numberOfItems = 10;
            setExemplaryCounterpartysStock(2);
            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            Assert.ThrowsException<ArgumentException>(() => tranche.NumberOfItems = -30);
        }

        [TestMethod()]
        public void TrancheQuotDiscountTest()
        {
            int numberOfItems = 22;
            setExemplaryCounterpartysStock(7);
            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            tranche.QuotaDiscount = 1;
            Assert.AreEqual(tranche.PriceNetto, 132);
        }

        [TestMethod()]
        public void TrancheQuotDiscountTest1()
        {
            Tranche tranche = getExemplaryTrancheWithDisounts(10, 50, 0.1, 0);
            tranche.QuotaDiscount = 2;
            Assert.AreEqual(tranche.PriceNetto, 300);
        }

        [TestMethod()]
        public void TrancheQuotDiscountTest2()
        {
            Tranche tranche = getExemplaryTrancheWithDisounts(10, 50, 0.1, 0);
            tranche.QuotaDiscount = 3;
            Assert.AreEqual(tranche.PriceNetto, 250);
        }

        [TestMethod()]
        public void TrancheQuotDiscountTest3()
        {
            int numberOfItems = 22;
            setExemplaryCounterpartysStock(7);
            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            tranche.QuotaDiscount = 400;
            Assert.AreEqual(tranche.PriceNetto, 0.01 * numberOfItems);
        }

        [TestMethod()]
        public void TrancheQuotDiscountTest4()
        {
            Tranche tranche = getExemplaryTrancheWithDisounts(10, 5, 0.4, 0);
            Double priceNettoBeforeQD = tranche.PriceNetto;
            Assert.ThrowsException<ArgumentException>(() => tranche.QuotaDiscount = -10);
            Assert.AreEqual(tranche.PriceNetto, priceNettoBeforeQD);
        }

        [TestMethod()]
        public void TrancheQuotDiscountTest5()
        {
            Tranche tranche = getExemplaryTrancheWithDisounts(10, 5, 0.4, 0);
            tranche.QuotaDiscount = 10;
            Assert.AreEqual(tranche.PriceNetto, 0.01 * 5);
        }

        [TestMethod()]
        public void TrancheDiscountsTest()
        {
            int numberOfItems = 10;
            setExemplaryCounterpartysStock(124);
            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            tranche.Discounts = getExemplaryPercentageDiscounts(0.3).Select(t => t.Object).ToList();
            Assert.AreEqual(tranche.PriceNetto, 496);
        }

        [TestMethod()]
        public void TrancheDiscountsTest1()
        {
            int numberOfItems = 10;
            setExemplaryCounterpartysStock(0.05);
            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            double priceNettoWithoutDiscounts = tranche.PriceNetto;
            tranche.Discounts = getExemplaryPercentageDiscounts(0.45).Select(t => t.Object).ToList();
            Assert.AreEqual(0.01 * numberOfItems, tranche.PriceNetto);
        }

        [TestMethod()]
        public void TrancheDiscountsTest2()
        {
            int numberOfItems = 10;
            setExemplaryCounterpartysStock(124);
            Tranche tranche = new Tranche(1, counterpartysStockMock.Object, numberOfItems);
            tranche.QuotaDiscount = 50; 
            tranche.Discounts = getExemplaryPercentageDiscounts(0.3).Select(t => t.Object).ToList();
            Assert.AreEqual(0.01 * numberOfItems, tranche.PriceNetto);
        }

        [TestMethod()]
        public void TranchePriceNettoTest()
        {
            int stocksPrice = random.Next(1, 20);
            int numberOfItems = random.Next(1, 20);
            setExemplaryCounterpartysStock(stocksPrice);
            var tranche = new Tranche(null, counterpartysStockMock.Object, numberOfItems);
            Assert.AreEqual(tranche.PriceNetto, stocksPrice * numberOfItems);
        }

        [TestMethod()]
        public void TranchePriceNettoTest1()
        {
            Tranche tranche = null;
            int numberOfItems = random.Next(-20, 200);
            try
            {
                double stocksPrice = random.Next(-10, 200) + Math.Round(random.NextDouble(), 2);
                double quotaDiscount = random.Next(-10, 300) + Math.Round(random.NextDouble(), 2);
                double amountForPercentageDiscounts = Math.Round(random.NextDouble(), 2) + 0.01;
                setExemplaryCounterpartysStock(stocksPrice);
                tranche = new Tranche(null, counterpartysStockMock.Object, numberOfItems, null, quotaDiscount,
                    getExemplaryPercentageDiscounts(amountForPercentageDiscounts).Select(d => d.Object).ToList());
            }
            catch(Exception) { }
            if(tranche !=  null)
                Assert.IsTrue(tranche.PriceNetto >= numberOfItems * 0.01);
        }
        

        private void setExemplaryCounterpartysStock(double stocksPrice)
        {

            counterpartysStockMock.CallBase = true;
            counterpartysStockMock.Setup(c => c.PriceNetto).Returns(stocksPrice);

        }

        private Tranche getExemplaryTrancheWithDisounts(double stockPrice, int numberOfItems, 
            double amountForBothPercentageDiscounts, int quotaDiscount)
        {
            counterpartysStockMock.CallBase = true;
            counterpartysStockMock.Setup(c => c.PriceNetto).Returns(stockPrice);
            var tranche = new Tranche(null, counterpartysStockMock.Object, numberOfItems);
            tranche.Discounts = getExemplaryPercentageDiscounts(amountForBothPercentageDiscounts).Select(pdMock => pdMock.Object).ToList();
            if(quotaDiscount != 0)
                tranche.QuotaDiscount = quotaDiscount;
            return tranche;
        }

        private List<Mock<PercentageDiscount>> getExemplaryPercentageDiscounts(double amountForBothPercentageDiscounts)
        {
            List<Mock<PercentageDiscount>> percentageDiscounts = new List<Mock<PercentageDiscount>>();
            //(int id, DateTime since, DateTime until, double sumNetto, double amount,
            //bool summing, bool active, List < CounterpartysStock > counterpartysStock)
            DateTime dateSince = new DateTime(2018, 1, 1);
            DateTime dateUntil = new DateTime(2018, 7, 1);
            var counterpartysStockList = new List<Mock<CounterpartysStock>>();
            counterpartysStockList.Add(counterpartysStockMock);
            percentageDiscounts.Add(new Mock<PercentageDiscount>(1, dateSince, dateUntil,
                1, amountForBothPercentageDiscounts, false, true, counterpartysStockList.Select(cs => cs.Object).ToList()));
            percentageDiscounts[0].CallBase = true;
            percentageDiscounts[0].Setup(pd => pd.Amount).Returns(amountForBothPercentageDiscounts);
            percentageDiscounts.Add(new Mock<PercentageDiscount>(2, dateSince, dateUntil,
                1, amountForBothPercentageDiscounts, false, true, counterpartysStockList.Select(cs => cs.Object).ToList()));
            percentageDiscounts[1].CallBase = true;
            percentageDiscounts[1].Setup(pd => pd.Amount).Returns(amountForBothPercentageDiscounts);
            return percentageDiscounts;
        }
    }
}
