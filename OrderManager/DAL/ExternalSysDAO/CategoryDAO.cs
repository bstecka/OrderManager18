using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.ExternalSysDAO
{
    class CategoryDAO : ReaderDAO, ICategoryDAO
    {
        public CategoryDAO() : base("Kategoria")
        {
        }
    }
}
