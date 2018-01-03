using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    class Stock : ReaderDAO, IStockDAO
    {
        public Stock() : base("Towar") { }

        public DataTable GetStocksActiveOrders(DataTable stock)
        {
            return DBOperations.Select(@"SELECT * FROM Zamowienie WHERE 
                ID in (SELECT ZamowienieID FROM Transza WHERE TowarKontrahenta)");
        }

        public DataTable GetStocksCounterpartysStock(DataTable stock)
        {
            return DBOperations.Select(@"SELECT * FROM TowarKontrahenta 
                WHERE TowarID IN (" + stock.Rows[0]["ID"] + ")");
        }

        public DataTable GetStocksCategory(DataTable stock)
        {
            return DBOperations.Select(@"SELECT * FROM Kategoria JOIN Towar
                ON Kategoria.ID = Towar.KategoriaID
                WHERE Towar.ID IN (" + stock.Rows[0]["ID"] + ")");
        }
    }
}
