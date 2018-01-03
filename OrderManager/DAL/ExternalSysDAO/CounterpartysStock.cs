using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.ExternalSysDAO
{
    class CounterpartysStock : ReaderDAO, ICounterpartysStockDAO
    {
        public CounterpartysStock() : base("TowarKontrahenta") { }

        public DataTable GetCounterparty(DataTable counterpartysStock)
        {
           return DBOperations.Select("SELECT * FROM Kontrahent WHERE ID IN ("
                + counterpartysStock.Rows[0]["KontrahentID"] + ")");
        }

        public DataTable GetCounterparty(DataRow counterpartysStock)
        {
            return DBOperations.Select("SELECT * FROM Kontrahent WHERE ID IN ("
               + counterpartysStock["KontrahentID"] + ")");
        }

        public DataTable GetPercentageDiscounts(DataTable counterpartysStock)
        {
            throw new NotImplementedException();
        }

        public DataTable GetStock(DataTable counterpartysStock)
        {
            return DBOperations.Select("SELECT * FROM Towar WHERE ID IN ("
                + counterpartysStock.Rows[0]["TowarID"] + ")");
        }

        public DataTable GetStock(DataRow counterpartysStock)
        {
            return DBOperations.Select("SELECT * FROM Towar WHERE ID IN ("
               + counterpartysStock["TowarID"] + ")");
        }
    }
}
