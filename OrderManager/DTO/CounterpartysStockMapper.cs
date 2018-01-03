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

        public CounterpartysStockMapper(ICounterpartysStockDAO counterpartysStockDAO)
        {
            this.counterpartysStockDAO = counterpartysStockDAO;
        }

        public List<Domain.Entity.CounterpartysStock> MapAllFrom(DataTable counterpartysStockDAO)
        {
            List<Domain.Entity.CounterpartysStock> result = new List<Domain.Entity.CounterpartysStock>();
            foreach (DataRow counterpartyRow in counterpartysStockDAO.Rows)
                result.Add(MapFrom(counterpartysStockDAO,
                    counterpartysStockDAO.Rows.IndexOf(counterpartyRow)));
            return result;
        }

        public Domain.Entity.CounterpartysStock MapFrom(DataTable counterpartysStockTable)
        {
            return MapFrom(counterpartysStockTable, 0);
        }

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
