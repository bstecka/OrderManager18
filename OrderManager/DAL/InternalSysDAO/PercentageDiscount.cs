using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    class PercentageDiscount : ReaderAndWriterDAO, IPercentageDiscountDAO
    {
        public PercentageDiscount() : base("RabatProcentowy") { }

        public DataTable GetCounterpartiesStockWithDiscount(DataTable discount)
        {
            if (discount.Rows.Count != 1 && !discount.Columns.Contains("ID"))
                throw new ArgumentOutOfRangeException();
            return DBOperations.Select(
                @"SELECT * FROM TowarKontrahenta WHERE ID IN
                (SELECT TowarKontrahentaID FROM RabatProcentowy_TowarKontrahenta
                WHERE RabatProcentowyID = " + discount.Rows[0]["ID"].ToString() + ")");
        }


        public DataTable GetCounterpartiesStockWithDiscount(DataRow discount)
        {
            return DBOperations.Select(
                @"SELECT * FROM TowarKontrahenta WHERE ID IN
                (SELECT TowarKontrahentaID FROM RabatProcentowy_TowarKontrahenta
                WHERE RabatProcentowyID = " + discount["ID"].ToString() + ")");
        }

        public DataTable GetCounterpartysStockValidDicounts(DataTable counterpartysStock)
        {
            
            if (counterpartysStock.Rows.Count != 1 && !counterpartysStock.Columns.Contains("ID"))
                throw new ArgumentOutOfRangeException();
            return DBOperations.Select(
              @"SELECT * FROM RabatProcentowy WHERE ID IN 
              (SELECT RabatProcentowyID FROM RabatProcentowy_TowarKontrahenta 
              WHERE TowarKontrahentaID = " + counterpartysStock.Rows[0]["ID"].ToString() + @")
              AND OdKiedy <= GETDATE() AND DoKiedy >= GETDATE() AND Aktywny = 1");
        }
    }
}
