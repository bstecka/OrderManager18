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
            return GetPriority(stock.Rows[0]);
        }

        public DataTable GetPriority(DataRow stock)
        {
            return DBOperations.Select(@"SELECT ListaKryteriow 
            FROM PriorytetTowaru JOIN Priorytet 
            ON  PriorytetTowaru.PriorytetID = Priorytet.ID
            WHERE TowarID in (" + stock["ID"] + ")");
        }
    }
}
