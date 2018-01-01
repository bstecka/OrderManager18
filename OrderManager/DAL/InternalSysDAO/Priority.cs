using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    class Priority : ReaderAndWriterDAO, IPriorityDAO
    {
        public Priority() : base("PriorytetTowaru") { }

        public DataTable GetPriority(DataTable stock)
        {
            throw new NotImplementedException();
        }

        public DataTable GetPriority(DataRow stock)
        {
            
            return DBOperations.Select(@"(SELECT PriorytetTowaruID AS ID FROM
                PriorytetTowaru WHERE TowarID in (" + stock["ID"] + "))" +
                " NATURAL JOIN Priorytet");
                
        }
    }
}
