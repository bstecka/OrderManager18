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
    class StockMapper : IMapperBase<Stock>
    {
        /// <summary>
        /// Maps from the dataTable to a list of stock entities.
        /// </summary>
        /// <param name="stockTable">The stock DataTable.</param>
        /// <returns>Returns a list of stock entities.</returns>
        public List<Stock> MapAllFrom(DataTable stockTable)
        {
            List<Stock> result = new List<Stock>();
            foreach (DataRow row in stockTable.Rows)
                result.Add(MapFrom(stockTable,
                    stockTable.Rows.IndexOf(row)));
            return result;
        }

        /// <summary>
        /// Maps from the dataTable to a stock entity.
        /// </summary>
        /// <param name="stockTable">The stock table.</param>
        /// <returns>Returns a stock entity.</returns>
        public Stock MapFrom(DataTable stockTable)
        {
            return MapFrom(stockTable, 0);
        }

        /// <summary>
        /// Maps from the stock entity to a DataTable.
        /// </summary>
        /// <param name="discountDomain">The stock enity.</param>
        /// <returns>Returns a DataTable with values corresponding to the stock entity.</returns>
        public DataTable MapTo(Stock stockDomain)
        {
            return MapTo(stockDomain, null);
        }

        /// <summary>
        /// Maps from the stock entity to a DataTable, with added values from the dataTableToAdd.
        /// </summary>
        /// <param name="stockDomain">The stock enity.</param>
        /// <returns>Returns a DataTable with values corresponding to the stock entity.</returns>
        public DataTable MapTo(Stock stockDomain, DataTable dataTableToAdd)
        {
            if(dataTableToAdd == null)
            {
                dataTableToAdd = new DataTable();
                dataTableToAdd.Columns.Add("ID");
                dataTableToAdd.Columns.Add("MinimumMagazynowe");
                dataTableToAdd.Columns.Add("WagaSztuki");
                dataTableToAdd.Columns.Add("MaksLiczbaSztukNaEuropalecie");
                dataTableToAdd.Columns.Add("LiczbaSztuk");
                dataTableToAdd.Columns.Add("MaksimumMagazynowe");
                dataTableToAdd.Columns.Add("VAT");
                dataTableToAdd.Columns.Add("Kod");
                dataTableToAdd.Columns.Add("Nazwa");
                dataTableToAdd.Columns.Add("KategoriaID");
            }
            DataRow dataRow = dataTableToAdd.NewRow();
            dataRow["ID"] = stockDomain.Id;
            dataRow["MinimumMagazynowe"] = stockDomain.MinInStockRoom;
            dataRow["WagaSztuki"] = stockDomain.WeightOfItem;
            dataRow["MaksLiczbaSztukNaEuropalecie"] = stockDomain.MaxNumberOfItemsOnEuropallet;
            dataRow["LiczbaSztuk"] = stockDomain.NumberOfItemsInStockRoom;
            dataRow["MaksimumMagazynowe"] = stockDomain.MaxInStockRoom;
            dataRow["VAT"] = stockDomain.VAT;
            dataRow["Kod"] = stockDomain.Code;
            dataRow["Nazwa"] = stockDomain.Name;
            dataRow["KategoriaID"] = stockDomain.Category.Id;
            dataTableToAdd.Rows.Add(dataRow);
            return dataTableToAdd;
        }

        /// <summary>
        /// Maps from a specified row of the dataTable to a stock entity.
        /// </summary>
        /// <param name="stockTable">The stock table.</param>
        /// <param name="numberOfRow">The number of row.</param>
        /// <returns>Returns the stock entity.</returns>
        private Stock MapFrom(DataTable stockTable, int numberOfRow)
        {
            DataRow stockRow = stockTable.Rows[numberOfRow];
            IStockDAO stockDAO = new StockDAO();
            DataRow categoryRow = stockDAO.GetStocksCategory(stockRow).Rows[0];
            Stock stock = new Stock(
            Convert.ToInt32(stockRow["ID"]),
            Convert.ToInt32(stockRow["MaksimumMagazynowe"]),
            Convert.ToInt32(stockRow["MinimumMagazynowe"]),
            Convert.ToInt32(stockRow["LiczbaSztuk"]),
            Convert.ToInt32(stockRow["WagaSztuki"]),
            0,
            Convert.ToInt32(stockRow["VAT"]),
            Convert.ToString(stockRow["Kod"]),
            Convert.ToString(stockRow["Nazwa"]),
            new Category(Convert.ToInt32(categoryRow["ID"]), Convert.ToString(categoryRow["NazwaKategorii"])),
            Convert.ToBoolean(stockRow["WGenerowanymZamowieniu"]));
            return stock;
        }

    }
}
