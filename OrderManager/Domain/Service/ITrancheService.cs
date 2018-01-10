using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    public interface ITrancheService : IEntityServiceBase<Tranche>
    {
        void UpdateTranche(Tranche tranche);
        int InsertTranche(Tranche tranche);
    }
}
