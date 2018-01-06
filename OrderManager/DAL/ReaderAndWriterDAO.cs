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

        public void Add(DataTable entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            DBOperations.Query("DELETE FROM " + tableName + " WHERE ID = " + id);
        }

        public void Update(DataTable entity)
        {
            DBOperations.Update(entity, tableName);
        }
    }
}
