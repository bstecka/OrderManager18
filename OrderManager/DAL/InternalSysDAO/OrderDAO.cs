using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    public class OrderDAO : ReaderAndWriterDAO, IOrderDAO
    {
        public OrderDAO() : base("Zamowienie") { }

        public DataTable GetCounterparty(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM Kontrahent 
                WHERE ID IN (" + order["KontrahentID"] + ")");
        }

        public DataTable GetOrderState(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM StanZamowienia 
                WHERE ID IN (" + order["StanZamowieniaID"] + ")");
        }

        public DataTable GetParentOrder(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie
                WHERE ID IN (SELECT ZamowienieNadrzedne FROM Zamowienie WHERE ID IN (" + order["ID"] + ")");
        }

        public DataTable GetParentOrderById(int id)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie
                WHERE ID IN (SELECT ZamowienieNadrzedne FROM Zamowienie WHERE ID IN ("+ id + "))");
        }

        public DataTable GetTranches(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM Transza 
                WHERE ZamowienieID IN (" + order["ID"] + ")");
        }

        public DataTable GetUser(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM Uzytkownik 
                WHERE ID IN (" + order["UzytkownikID"] + ")");
        }

        //Nie umiem w architekturę więc nie wiem czy to na pewno powinno być tutaj, ale gdyby to wrzucić do service'u,
        //to trzeba by było mapować wszystkie zamówienia, żeby tylko wyciągnąć te w trakcie realizacji
        public DataTable GetAllDuringRealization() 
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie 
                WHERE StanZamowieniaID = 1");
        }

        public DataTable GetAllByState(int stateId)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie 
                WHERE StanZamowieniaID IN (" + stateId + ")");
        }
    }
}
