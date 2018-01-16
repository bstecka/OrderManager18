using OrderManager.DAL.ExternalSysDAO;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DTO
{
    class CounterpartysStockMapper : IMapperBase<Domain.Entity.CounterpartysStock>
    {
        ICounterpartysStockDAO counterpartysStockDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartysStockMapper"/> class.
        /// </summary>
        /// <param name="counterpartysStockDAO">The counterpartys stock DAO.</param>
        public CounterpartysStockMapper(ICounterpartysStockDAO counterpartysStockDAO)
        {
            this.counterpartysStockDAO = counterpartysStockDAO;
        }

        /// <summary>
        /// Maps from the dataTable to a list of counterparty stock entities.
        /// </summary>
        /// <param name="counterpartysStockDAO">The counterpartys stock DAO.</param>
        /// <returns>The list of counterparty stock entities.</returns>
        public List<Domain.Entity.CounterpartysStock> MapAllFrom(DataTable counterpartysStockDAO)
        {
            List<Domain.Entity.CounterpartysStock> result = new List<Domain.Entity.CounterpartysStock>();
            foreach (DataRow counterpartyRow in counterpartysStockDAO.Rows)
                result.Add(MapFrom(counterpartysStockDAO,
                    counterpartysStockDAO.Rows.IndexOf(counterpartyRow)));
            return result;
        }

        /// <summary>
        /// Maps from a dataTable to counterparty stock entity.
        /// </summary>
        /// <param name="counterpartysStockTable">The counterpartys stock table.</param>
        /// <returns>Counterparty stock entity</returns>
        public Domain.Entity.CounterpartysStock MapFrom(DataTable counterpartysStockTable)
        {
            return MapFrom(counterpartysStockTable, 0);
        }

        /// <summary>
        /// Maps from a specified row of dataTable to counterparty stock entity.
        /// </summary>
        /// <param name="counterpartysStockTable">The counterpartys stock table.</param>
        /// <param name="numberOfRow">The number of row.</param>
        /// <returns>Counterparty stock entity</returns>
        public Domain.Entity.CounterpartysStock MapFrom(DataTable counterpartysStockTable, int numberOfRow)
        {
            DataRow counterpartysStockRow = counterpartysStockTable.Rows[numberOfRow];
            var counterparty = (new CounterpartyMapper()).MapFrom(
                counterpartysStockDAO.GetCounterparty(counterpartysStockRow));
            var stock = (new StockMapper()).MapFrom(
                counterpartysStockDAO.GetStock(counterpartysStockRow));
            return new Domain.Entity.CounterpartysStock(
                Convert.ToInt32(counterpartysStockRow["ID"]),
                stock,
                counterparty,
                Convert.ToDouble(counterpartysStockRow["CenaNetto"]));
        }

        /// <summary>
        /// Maps from the counterparty stock entity to a DataTable.
        /// </summary>
        /// <param name="counterpartsStockyDomain">The counterparts stocky domain.</param>
        /// <returns>Returns a DataTable with values corresponding to the counterparty entity.</returns>
        public DataTable MapTo(Domain.Entity.CounterpartysStock counterpartsStockyDomain)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("TowarID");
            dataTable.Columns.Add("KontahentID");
            dataTable.Columns.Add("CenaNetto");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ID"] = counterpartsStockyDomain.Id;
            dataRow["TowarID"] = counterpartsStockyDomain.Stock.Id;
            dataRow["KontahentID"] = counterpartsStockyDomain.Counterparty.Id;
            dataRow["CenaNetto"] = counterpartsStockyDomain.PriceNetto;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }
    }
}
