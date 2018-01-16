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
        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityDAO"/> class.
        /// </summary>
        public PriorityDAO() : base("PriorytetTowaru") { }

        /// <summary>
        /// Gets the priority of given stock.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the DataTable containing the priority of given stock.</returns>
        public DataTable GetPriority(DataTable stock)
        {
            return GetPriority(stock.Rows[0]);
        }

        /// <summary>
        /// Gets the priority of given stock.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the DataTable containing the priority of given stock.</returns>
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
