using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.ExternalSysDAO
{
    class CounterpartysStockDAO : ReaderDAO, ICounterpartysStockDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartysStockDAO"/> class.
        /// </summary>
        public CounterpartysStockDAO() : base("TowarKontrahenta") { }

        /// <summary>
        /// Gets the counterparty for given counterpartys stock.
        /// </summary>
        /// <param name="counterpartysStock">The counterpartys stock.</param>
        /// <returns>Returns the DataTable of counterparty from given counterpartys stock.</returns>
        public DataTable GetCounterparty(DataTable counterpartysStock)
        {
           return DBOperations.Query("SELECT * FROM Kontrahent WHERE ID IN ("
                + counterpartysStock.Rows[0]["KontrahentID"] + ")");
        }

        /// <summary>
        /// Gets the counterparty for given counterpartys stock.
        /// </summary>
        /// <param name="counterpartysStock">The counterpartys stock.</param>
        /// <returns>Returns the DataTable of counterparty from given counterpartys stock.</returns>
        public DataTable GetCounterparty(DataRow counterpartysStock)
        {
            return DBOperations.Query("SELECT * FROM Kontrahent WHERE ID IN ("
               + counterpartysStock["KontrahentID"] + ")");
        }

        /// <summary>
        /// Gets the stock for given counterpartys stock.
        /// </summary>
        /// <param name="counterpartysStock">The counterpartys stock.</param>
        /// <returns>Returns the DataTable of stock for given counterpartys stock.</returns>
        public DataTable GetStock(DataTable counterpartysStock)
        {
            return DBOperations.Query("SELECT * FROM Towar WHERE ID IN ("
                + counterpartysStock.Rows[0]["TowarID"] + ")");
        }

        /// <summary>
        /// Gets the stock for given counterpartys stock.
        /// </summary>
        /// <param name="counterpartysStock">The counterpartys stock.</param>
        /// <returns>Returns the DataTable of stock for given counterpartys stock.</returns>
        public DataTable GetStock(DataRow counterpartysStock)
        {
            return DBOperations.Query("SELECT * FROM Towar WHERE ID IN ("
               + counterpartysStock["TowarID"] + ")");
        }

        /// <summary>
        /// Gets valid dicounts for the counterpartys stock.
        /// </summary>
        /// <param name="counterpartysStock">The counterpartys stock.</param>
        /// <returns>Returns the DataTable of valid dicounts for the counterpartys stock.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DataTable GetCounterpartysStockValidDicounts(DataTable counterpartysStock)
        {
            if (counterpartysStock.Rows.Count != 1 && !counterpartysStock.Columns.Contains("ID"))
                throw new ArgumentOutOfRangeException();
            return DBOperations.Query(
              @"SELECT * FROM RabatProcentowy WHERE ID IN 
              (SELECT RabatProcentowyID FROM RabatProcentowy_TowarKontrahenta 
              WHERE TowarKontrahentaID = " + counterpartysStock.Rows[0]["ID"].ToString() + @")
              AND OdKiedy <= GETDATE() AND DoKiedy >= GETDATE() AND Aktywny = 1");
        }
    }
}
