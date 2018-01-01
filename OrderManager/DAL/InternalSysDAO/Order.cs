using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    class Order : ReaderAndWriterDAO, IOrderDAO
    {
        public Order() : base("Zamowienie") { }

        public DataTable GetCounterparty(DataRow order)
        {
            return DBOperations.Select(@"SELECT * FROM Kontrahent 
                WHERE ID IN (" + order["KontrahentID"] + ")");
        }

        public DataTable GetOrderState(DataRow order)
        {
            return DBOperations.Select(@"SELECT * FROM StanZamowienia 
                WHERE ID IN (" + order["StanZamowieniaID"] + ")");
        }

        public DataTable GetParentOrder(DataRow order) ////////////////EXCEPTIONS
        {
            return DBOperations.Select(@"SELECT * FROM Zamowienie 
                WHERE ZamowienieID IN (" + order["ZamowienieNadrzedne"] + ")");
        }

        public DataTable GetTranches(DataRow order)
        {
            return DBOperations.Select(@"SELECT * FROM Transza 
                WHERE ZamowienieID IN (" + order["ID"] + ")");
        }

        public DataTable GetUser(DataRow order)
        {
            return DBOperations.Select(@"SELECT * FROM Uzytkownik 
                WHERE ID IN (" + order["UzytkownikID"] + ")");
        }
    }
}
