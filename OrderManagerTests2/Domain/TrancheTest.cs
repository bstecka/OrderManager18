using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            stockMock = new Mock<Stock>(1, 1, 1, 1, 1, 1, 23, "Code", "TestStock", null);
            counterpartyMock = new Mock<Counterparty>(1, 11111111111, "TestCounterparty", 1);
            counterpartysStockMock = new Mock<CounterpartysStock>(1, stockMock.Object, counterpartyMock.Object, 1);
            random = new Random();
        }

        [TestMethod()]
        public void TrancheTest()
        {
            int stocksPrice = random.Next(1, 20);
            int numberOfItems = random.Next(1, 20);
            counterpartysStockMock.CallBase = true;
            counterpartysStockMock.Setup(c => c.PriceNetto).Returns(stocksPrice);

            var tranche = new Tranche(null, counterpartysStockMock.Object, numberOfItems);
            Assert.AreEqual(tranche.PriceNetto, stocksPrice * numberOfItems);
        }

        [TestMethod()]
        public void TrancheTest1()
        {
            int stocksPrice = 0;
            int numberOfItems = random.Next(0, 20);
            counterpartysStockMock.CallBase = true;
            counterpartysStockMock.Setup(c => c.PriceNetto).Returns(stocksPrice);

            var tranche = new Tranche(null, counterpartysStockMock.Object, numberOfItems);
            Assert.AreEqual(tranche.PriceNetto, 0);
        }

        [TestMethod()]
        public void TrancheTest2()
        {
            int stocksPrice = random.Next(1, 20);
            int numberOfItems = random.Next(1, 20);
            counterpartysStockMock.CallBase = true;
            counterpartysStockMock.Setup(c => c.PriceNetto).Returns(stocksPrice);
            var tranche = new Tranche(null, counterpartysStockMock.Object, numberOfItems);

            int quotaDiscount = 0;
            tranche.QuotaDiscount = quotaDiscount;
            var prevPriceNetto = tranche.PriceNetto;
            while ((quotaDiscount = random.Next(1, 400)) > tranche.PriceNetto) { }
            tranche.QuotaDiscount = quotaDiscount;

            Assert.IsTrue(prevPriceNetto > tranche.PriceNetto);
        }

        [TestMethod()]
        public void TrancheTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TrancheTest4()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.Fail();
        }
    }
}
