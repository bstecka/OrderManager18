using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    class StockDAO : ReaderDAO, IStockDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StockDAO"/> class.
        /// </summary>
        public StockDAO() : base("Towar") { }

        /// <summary>
        /// Gets the stocks active orders.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the DataTable of active orders for the given stock.</returns>
        public DataTable GetStocksActiveOrders(DataTable stock)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie WHERE 
                ID in (SELECT ZamowienieID FROM Transza JOIN TowarKontrahenta
                ON Transza.TowarKontrahentaID = TowarKontrahenta.ID
                WHERE TowarID = " + stock.Rows[0]["ID"] + ") AND StanZamowieniaID = 1");
        }

        /// <summary>
        /// Gets the counterpartys stock for the given stock.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the DataTable of counterpartys stock for the given stock</returns>
        public DataTable GetStocksCounterpartysStock(DataTable stock)
        {
            return DBOperations.Query(@"SELECT * FROM TowarKontrahenta 
                WHERE TowarID IN (" + stock.Rows[0]["ID"] + ")");
        }

        /// <summary>
        /// Gets the category for the given stock.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the DataTable containing the category of the given stock.</returns>
        public DataTable GetStocksCategory(DataRow stock)
        {
            return DBOperations.Query("SELECT * FROM Kategoria WHERE ID = " + stock["KategoriaID"]);
        }

        /// <summary>
        /// Sets the possibility to generate order.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns a boolean representing thepossibility to generate order.</returns>
        public bool SetPossibilityToGenerateOrder(int id, int value) 
            //Gdyby to byl oracle, to bym zrobila triggera before update i rzucala z niego wyjatek, jak checmy true 
            //zmienic na true, tu bym go lapala i w catchu zwracal false, ale mssql to zlo jak caly microsoft, wiec moge tylko plakac.
        {
            DBOperations.OpertionThatRequiresOpeningDBConnection(@"UPDATE Towar SET WGenerowanymZamowieniu = " 
            + value + " WHERE ID = " + id);
            return true; 
        }
        
    }
}
