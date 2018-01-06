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

        public ReaderDAO(string tableName)
        {
            this.tableName = tableName;
        }

        public bool Exist(string id)
        {
            return DBOperations.Query("SELECT * FROM " + tableName + " WHERE ID = " + id).Rows.Count > 0;
        }

        public DataTable GetAll()
        {
            return DBOperations.Query("SELECT * FROM " + tableName);
        }

        public DataTable GetById(string id)
        {
            return DBOperations.Query("SELECT * FROM " + tableName + " WHERE ID = " + id);
        }
    }
}
