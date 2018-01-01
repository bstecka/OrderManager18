using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    interface IPriorityDAO : IReadableDAO, IWritableDAO
    {
        DataTable GetPriority(DataTable stock);
        DataTable GetPriority(DataRow stock);
    }
}
