using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL
{
    interface IReadableDAO
    {
        //DataTable GetWhereFieldEqual(DataTable parameters);
        DataTable GetById(string id);
        DataTable GetAll();
        bool Exist(string id);
    }
}
