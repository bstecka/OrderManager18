using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL
{
    public class ReaderAndWriterDAO : ReaderDAO, IWritableDAO
    {
        public ReaderAndWriterDAO(string tableName) : base(tableName) { }

        /// <summary>
        /// Insert the given value into the chosen table in the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns the id of the inserted row.</returns>
        public int Add(DataTable entity)
        {
            return DBOperations.Insert(entity, tableName);
        }

        /// <summary>
        /// Removes the row with the specified identifier from the chosen table in the database.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Remove(int id)
        {
            DBOperations.Query("DELETE FROM " + tableName + " WHERE ID = " + id);
        }

        /// <summary>
        /// Updates the row with the specified identifier in the chosen table in the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(DataTable entity)
        {
            DBOperations.Update(entity, tableName);
        }
    }
}
