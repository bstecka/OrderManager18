using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL
{
    interface IWritableDAO
    {
        void Add(DataTable entity);
        void Update(DataTable entity);
        void Remove(int id);
    }
}
