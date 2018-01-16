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
        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartyDAO"/> class.
        /// </summary>
        public CounterpartyDAO() : base("Kontrahent") { }

        /// <summary>
        /// Gets the counterpartys orders.
        /// </summary>
        /// <param name="counterparty">The counterparty.</param>
        /// <returns>Returns a DataTable of counterpartys orders.</returns>
        public DataTable GetCounterpartysOrders(DataTable counterparty)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie
                WHERE KontrahentID IN (" + counterparty.Rows[0]["ID"] + ")");
        }

        /// <summary>
        /// Gets the counterpartys stock.
        /// </summary>
        /// <param name="counterparty">The counterparty.</param>
        /// <returns>Returns the DataTable of counterpartys stock.</returns>
        public DataTable GetCounterpartysStock(DataTable counterparty)
        {
            return DBOperations.Query(@"SELECT * FROM TowarKontrahenta 
                WHERE KontrahentID IN (" + counterparty.Rows[0]["ID"] + ")");
        }
    }
}
