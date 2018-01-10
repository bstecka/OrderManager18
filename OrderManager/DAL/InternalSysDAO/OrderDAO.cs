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

        public void UpdateOrders(DataTable table)
        {
            DBOperations.Update(table, "Zamowienie");
        }

        public void UpdateOrder(DataRow row)
        {
            DataTable table = new DataTable();
            table.Rows.InsertAt(row, 0);
            DBOperations.Update(table, "Zamowienie");
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
