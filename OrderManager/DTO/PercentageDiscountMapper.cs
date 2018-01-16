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
    class PercentageDiscountMapper : IMapperBase<Domain.Entity.PercentageDiscount>
    {
        private IPercentageDiscountDAO percentageDiscountDAO;
        private ICounterpartysStockDAO counterpartysStockDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="PercentageDiscountMapper"/> class.
        /// </summary>
        /// <param name="percentageDiscountDAO">The percentage discount DAO.</param>
        /// <param name="counterpartysStockDAO">The counterpartys stock DAO.</param>
        public PercentageDiscountMapper(IPercentageDiscountDAO percentageDiscountDAO, ICounterpartysStockDAO counterpartysStockDAO)
        {
            this.percentageDiscountDAO = percentageDiscountDAO;
            this.counterpartysStockDAO = counterpartysStockDAO;
        }

        /// <summary>
        /// Maps from the dataTable to a list of percentageDiscount entities.
        /// </summary>
        /// <param name="discountTable">The discount table.</param>
        /// <returns>Returns a list of percentageDiscount entities.</returns>
        public List<Domain.Entity.PercentageDiscount> MapAllFrom(DataTable discountTable)
        {
            List<Domain.Entity.PercentageDiscount> result = new List<Domain.Entity.PercentageDiscount>();
            foreach (DataRow row in discountTable.Rows)
                result.Add(MapFrom(discountTable,
                    discountTable.Rows.IndexOf(row)));
            return result;
        }

        /// <summary>
        /// Maps from the dataTable to a percentageDiscount entity.
        /// </summary>
        /// <param name="discountTable">The percentageDiscount table.</param>
        /// <returns>Returns the percentageDiscount entity.</returns>
        public Domain.Entity.PercentageDiscount MapFrom(DataTable discountTable)
        {
            return MapFrom(discountTable, 0);
        }

        /// <summary>
        /// Maps from a specified row of the dataTable to a percentageDiscount entity.
        /// </summary>
        /// <param name="discountTable">The percentageDiscount table.</param>
        /// <param name="numberOfRow">The number of row.</param>
        /// <returns>Returns the percentageDiscount entity.</returns>
        private Domain.Entity.PercentageDiscount MapFrom(DataTable discountTable, int numberOfRow)
        {
            CounterpartysStockMapper counterpartysStockMapper = new CounterpartysStockMapper(counterpartysStockDAO);
            DataRow counterpartyRow = discountTable.Rows[numberOfRow];
            return new Domain.Entity.PercentageDiscount(
            Convert.ToInt32(counterpartyRow["ID"]),
            Convert.ToDateTime(counterpartyRow["OdKiedy"]),
            Convert.ToDateTime(counterpartyRow["DoKiedy"]),
            Convert.ToDouble(counterpartyRow["SumaWartosciPozycjiNetto"]),
            Convert.ToDouble(counterpartyRow["Wysokosc"]),
            Convert.ToBoolean(counterpartyRow["Sumowanie"]),
            Convert.ToBoolean(counterpartyRow["Aktywny"]),
            counterpartysStockMapper.MapAllFrom(percentageDiscountDAO.GetCounterpartiesStockWithDiscount(discountTable.Rows[numberOfRow])));
        }

        /// <summary>
        /// Maps from the percentageDiscount entity to a DataTable.
        /// </summary>
        /// <param name="discountDomain">The percentageDiscount enity.</param>
        /// <returns>Returns a DataTable with values corresponding to the percentageDiscount entity.</returns>
        public DataTable MapTo(Domain.Entity.PercentageDiscount discountDomain)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("OdKiedy");
            dataTable.Columns.Add("DoKiedy");
            dataTable.Columns.Add("SumaWartosciPozycjiNetto");
            dataTable.Columns.Add("Wysokosc");
            dataTable.Columns.Add("Sumowanie");
            dataTable.Columns.Add("Aktywny");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ID"] = discountDomain.Id;
            dataRow["OdKiedy"] = discountDomain.Since;
            dataRow["DoKiedy"] = discountDomain.Until;
            dataRow["SumaWartosciPozycjiNetto"] = discountDomain.SumNetto;
            dataRow["Wysokosc"] = discountDomain.Amount;
            dataRow["Sumowanie"] = discountDomain.Summing;
            dataRow["Aktywny"] = discountDomain.Active;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }


    }
}
