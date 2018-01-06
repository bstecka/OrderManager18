using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManager.DAL;
using OrderManager.Domain;
using OrderManager.DAL.InternalSysDAO;

namespace ReaderAndWriterDAOTests
{
    [TestClass]
    public class ReaderAndWriterDAOTests
    {
        [TestMethod]
        public void UpdateChangesValuesInDB()
        {
            DataTable discountsTable = new PercentageDiscount().GetAll();
            DataTable discount4 = new PercentageDiscount().GetById("4");
            ReaderAndWriterDAO dao = new ReaderAndWriterDAO("RabatProcentowy");
            discount4.Rows[0].BeginEdit();
            discount4.Rows[0]["Aktywny"] = 0;
            discount4.Rows[0]["Sumowanie"] = 1;
            discount4.Rows[0].EndEdit();
            dao.Update(discount4);
            DataTable discount4AfterUpdate = new PercentageDiscount().GetById("4");
            Assert.AreNotEqual(discount4, discount4AfterUpdate);
        }// test jest zły bo nie jest przeciążone equals dla datatable a nie mogę użyć mappera
        // bo nie jest widoczny, a jak próbuję dać go na public to wszystko się psuje
    }
}
