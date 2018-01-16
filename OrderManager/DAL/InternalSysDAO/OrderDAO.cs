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
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDAO"/> class.
        /// </summary>
        public OrderDAO() : base("Zamowienie") { }

        /// <summary>
        /// Gets the counterparty for the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>Returns the DataTable containing the data of the counterparty for the given order.</returns>
        public DataTable GetCounterparty(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM Kontrahent 
                WHERE ID IN (" + order["KontrahentID"] + ")");
        }

        /// <summary>
        /// Updates the orders in the given table inside the database.
        /// </summary>
        /// <param name="table">The table.</param>
        public void UpdateOrders(DataTable table)
        {
            DBOperations.Update(table, "Zamowienie");
        }

        /// <summary>
        /// Updates the order in the given row inside the database.
        /// </summary>
        /// <param name="row">The row.</param>
        public void UpdateOrder(DataRow row)
        {
            DataTable table = new DataTable();
            table.Rows.InsertAt(row, 0);
            DBOperations.Update(table, "Zamowienie");
        }

        /// <summary>
        /// Gets the state of the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>Returns the DataTable containing the state of the given order.</returns>
        public DataTable GetOrderState(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM StanZamowienia 
                WHERE ID IN (" + order["StanZamowieniaID"] + ")");
        }

        /// <summary>
        /// Gets the parent order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>Returns the DataTable containing the data of the parent order of the given order.</returns>
        public DataTable GetParentOrder(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie
                WHERE ID IN (SELECT ZamowienieNadrzedne FROM Zamowienie WHERE ID IN (" + order["ID"] + ")");
        }

        /// <summary>
        /// Gets the parent order by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns the DataTable containing the data of the parent order of the order with given identifier.</returns>
        public DataTable GetParentOrderById(int id)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie
                WHERE ID IN (SELECT ZamowienieNadrzedne FROM Zamowienie WHERE ID IN ("+ id + "))");
        }

        /// <summary>
        /// Gets the tranches assigned to given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>Returns the DataTable of the tranches assigned to given order.</returns>
        public DataTable GetTranches(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM Transza 
                WHERE ZamowienieID IN (" + order["ID"] + ")");
        }

        /// <summary>
        /// Gets the user who created the order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>Returns the DataTable containing the data of the user who created the given order.</returns>
        public DataTable GetUser(DataRow order)
        {
            return DBOperations.Query(@"SELECT * FROM Uzytkownik 
                WHERE ID IN (" + order["UzytkownikID"] + ")");
        }

        /// <summary>
        /// Gets all orders during realization.
        /// </summary>
        /// <returns>Returns a DataTable of all orders during realization.</returns>
        public DataTable GetAllDuringRealization() 
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie 
                WHERE StanZamowieniaID = 1");
        }

        /// <summary>
        /// Gets all orders by the state id.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>Returns the DataTable of all orders with state matching the given identifier.</returns>
        public DataTable GetAllByState(int stateId)
        {
            return DBOperations.Query(@"SELECT * FROM Zamowienie 
                WHERE StanZamowieniaID IN (" + stateId + ")");
        }

        /// <summary>
        /// Corrects the orders with the id of oldOrder into the value of newOrder.
        /// </summary>
        /// <param name="oldOrder">The old order.</param>
        /// <param name="newOrder">The new order.</param>
        public void OrderCorrection(DataTable oldOrder, DataTable newOrder)
        {
            DBOperations.UpdateAndSave(oldOrder, newOrder, tableName);
        }
    }
}
