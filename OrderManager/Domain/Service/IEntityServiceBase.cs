using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    public interface IEntityServiceBase<T>
    {
        T GetById(string id);
        List<T> GetAll();
    }
}
