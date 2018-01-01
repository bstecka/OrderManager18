using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL
{
    class ReaderAndWriterDAO : ReaderDAO, IWritableDAO
    {
        public ReaderAndWriterDAO(string tableName) : base(tableName) { }

        public void Add(DataTable entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(DataTable entity)
        {
            throw new NotImplementedException();
        }
    }
}
