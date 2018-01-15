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
        public StockDAO() : base("Towar") { }

        public DataTable GetStocksActiveOrders(DataTable stock)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie WHERE 
                ID in (SELECT ZamowienieID FROM Transza JOIN TowarKontrahenta
                ON Transza.TowarKontrahentaID = TowarKontrahenta.ID
                WHERE TowarID = " + stock.Rows[0]["ID"] + ") AND StanZamowieniaID = 1");
        }

        public DataTable GetStocksCounterpartysStock(DataTable stock)
        {
            return DBOperations.Query(@"SELECT * FROM TowarKontrahenta 
                WHERE TowarID IN (" + stock.Rows[0]["ID"] + ")");
        }

        public DataTable GetStocksCategory(DataRow stock)
        {
            /*
            return DBOperations.Query(@"SELECT * FROM Kategoria JOIN Towar
                ON Kategoria.ID = Towar.KategoriaID
                WHERE Towar.ID IN (" + stock.Rows[0]["ID"] + ")");*/
            return DBOperations.Query("SELECT * FROM Kategoria WHERE ID = " + stock["KategoriaID"]);
        }

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
