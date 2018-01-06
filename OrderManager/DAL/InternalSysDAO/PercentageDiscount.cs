using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    public class PercentageDiscount : ReaderAndWriterDAO, IPercentageDiscountDAO
    {
        public PercentageDiscount() : base("RabatProcentowy") { }

        public DataTable GetCounterpartiesStockWithDiscount(DataTable discount)
        {
            if (discount.Rows.Count != 1 && !discount.Columns.Contains("ID"))
                throw new ArgumentOutOfRangeException();
            return DBOperations.Query(
                @"SELECT * FROM TowarKontrahenta WHERE ID IN
                (SELECT TowarKontrahentaID FROM RabatProcentowy_TowarKontrahenta
                WHERE RabatProcentowyID = " + discount.Rows[0]["ID"].ToString() + ")");
        }

        public DataTable GetCounterpartiesStockWithDiscount(DataRow discount)
        {
            return DBOperations.Query(
                @"SELECT * FROM TowarKontrahenta WHERE ID IN
                (SELECT TowarKontrahentaID FROM RabatProcentowy_TowarKontrahenta
                WHERE RabatProcentowyID = " + discount["ID"].ToString() + ")");
        }
    }
}
