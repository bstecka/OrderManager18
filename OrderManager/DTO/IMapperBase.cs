using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DTO
{
    public interface IMapperBase<T>
    {
        T MapFrom(DataTable tDAO);
        DataTable MapTo(T tDomain);
        List<T> MapAllFrom(DataTable tDAO);
    }
}
