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
        public List<Stock> MapAllFrom(DataTable tDAO)
        {
            List<Stock> result = new List<Stock>();
            foreach (DataRow row in tDAO.Rows)
                result.Add(MapFrom(tDAO,
                    tDAO.Rows.IndexOf(row)));
            return result;
        }

        public Stock MapFrom(DataTable tDAO)
        {
            return MapFrom(tDAO, 0);
        }

        public DataTable MapTo(Stock tDomain)
        {
            return MapTo(tDomain, null);
        }

        public DataTable MapTo(Stock tDomain, DataTable dataTableToAdd)
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
            }
            DataRow dataRow = dataTableToAdd.NewRow();
            dataRow["ID"] = tDomain.Id;
            dataRow["MinimumMagazynowe"] = tDomain.MinInStockRoom;
            dataRow["WagaSztuki"] = tDomain.WeightOfItem;
            dataRow["MaksLiczbaSztukNaEuropalecie"] = tDomain.NumOfItems;
            dataRow["LiczbaSztuk"] = tDomain.NumberOfImtems;
            dataRow["MaksimumMagazynowe"] = tDomain.MaxInStockroom;
            dataRow["VAT"] = tDomain.VAT;
            dataRow["Kod"] = tDomain.Code;
            dataRow["Nazwa"] = tDomain.Name;
            dataTableToAdd.Rows.Add(dataRow);
            return dataTableToAdd;
        }

        private Domain.Entity.Stock MapFrom(DataTable stockTable, int numberOfRow)
        {
            DataRow stockRow = stockTable.Rows[numberOfRow];
            Domain.Entity.Stock counterparty = new Domain.Entity.Stock(
            Convert.ToInt32(stockRow["ID"]),
            Convert.ToInt32(stockRow["MaksimumMagazynowe"]),
            Convert.ToInt32(stockRow["MinimumMagazynowe"]),
            Convert.ToInt32(stockRow["LiczbaSztuk"]),
            Convert.ToInt32(stockRow["WagaSztuki"]),
            //Convert.ToInt32(stockRow["MaksLiczbaSztukNaEuropalecie"]),
            0,
            Convert.ToInt32(stockRow["VAT"]),
            Convert.ToString(stockRow["Kod"]),
            Convert.ToString(stockRow["Nazwa"]));
            return counterparty;
        }

    }
}
