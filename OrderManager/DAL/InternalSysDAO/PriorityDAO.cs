using OrderManager.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    class PriorityDAO : ReaderAndWriterDAO, IPriorityDAO
    {
        public PriorityDAO() : base("PriorytetTowaru") { }

        public DataTable GetPriority(DataTable stock)
        {
            return GetPriority(stock.Rows[0]);
        }

        public DataTable GetPriority(DataRow stock)
        {
            var stocksPriority = DBOperations.Query(@"SELECT ListaKryteriow 
            FROM PriorytetTowaru JOIN Priorytet 
            ON  PriorytetTowaru.PriorytetID = Priorytet.ID
            WHERE TowarID in (" + stock["ID"] + ")");
            if(stocksPriority.Rows.Count == 0)
            {
                var generalPriority = stocksPriority.NewRow();
                generalPriority[0] = Properties.Settings.Default["GeneralPriority"];
                stocksPriority.Rows.Add(generalPriority);
            }
            return stocksPriority;
        }
    }
}
