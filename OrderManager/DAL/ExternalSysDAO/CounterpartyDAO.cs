using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderManager.DAL;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.DAO;

namespace OrderManager.ExternalSysDAO
{
    class CounterpartyDAO : ReaderDAO, ICounterpartyDAO
    {
        public CounterpartyDAO() : base("Kontrahent") { }

        public DataTable GetCounterpartysOrders(DataTable counterparty)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie
                WHERE KontrahentID IN (" + counterparty.Rows[0]["ID"] + ")");
        }

        public DataTable GetCounterpartysStock(DataTable counterparty)
        {
            return DBOperations.Query(@"SELECT * FROM TowarKontrahenta 
                WHERE KontrahentID IN (" + counterparty.Rows[0]["ID"] + ")");
        }
    }
}
