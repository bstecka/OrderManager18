using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL
{
    public class ReaderDAO : IReadableDAO
    {
        protected string tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderDAO"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public ReaderDAO(string tableName)
        {
            this.tableName = tableName;
        }

        /// <summary>
        /// Checks if the item with a given id exists in the table with the given tableName in the database.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns a boolean representing if the item with a given id exists in the database.</returns>
        public bool Exist(string id)
        {
            return DBOperations.Query("SELECT * FROM " + tableName + " WHERE ID = " + id).Rows.Count > 0;
        }

        /// <summary>
        /// Gets all rows from the table with a given tableName in from the database.
        /// </summary>
        /// <returns>Returns a DataTable with all rows from the chosen table.</returns>
        public DataTable GetAll()
        {
            return DBOperations.Query("SELECT * FROM " + tableName);
        }

        /// <summary>
        /// Gets a rows from the table with a given tableName in from the database, with an id matching the given id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns a DataTable with one rows from the chosen table, where the row's id matches the given id.</returns>
        public DataTable GetById(string id)
        {
            return DBOperations.Query("SELECT * FROM " + tableName + " WHERE ID = " + id);
        }
    }
}
