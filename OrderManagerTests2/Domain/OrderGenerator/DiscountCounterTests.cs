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
    [TestClass()]
    public class DiscountCounterTests
    {
        [TestMethod()]
        public void BestChosenDiscountsTest()
        {
            Mock<ICounterpartysStockService> mockCounterpartysStockService = new Mock<ICounterpartysStockService>();
            Mock<IStockService> mockStockService = new Mock<IStockService>();

            var stockToOrder = new Dictionary<Entity.Stock, int>();
            //stodkToOrder.Add()

            DiscountCounter discountCounter = new DiscountCounter(stockToOrder, mockCounterpartysStockService.Object, mockStockService.Object);
            discountCounter.BestChosenDiscounts();

            mockCounterpartysStockService.Verify(m => m.GetValidDiscounts(It.IsAny<CounterpartysStock>()), Times.Exactly(0));
            //Assert.Fail();
        }
    }
}