﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;

namespace OrderManager.DTO
{
    class TrancheMapper : IMapperBase<Domain.Entity.Tranche>
    {
        ITrancheDAO trancheDAO;

        public TrancheMapper(ITrancheDAO trancheDAO)
        {
            this.trancheDAO = trancheDAO;
        }

        public DataTable MapTo(Domain.Entity.Tranche trancheDomain)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("ZamowienieID");
            dataTable.Columns.Add("LiczbaSztuk");
            dataTable.Columns.Add("RabatKwotowy");
            dataTable.Columns.Add("CenaNetto");
            dataTable.Columns.Add("CenaBrutto");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ID"] = trancheDomain.Id;
            dataRow["ZamowienieID"] = trancheDomain.OrderId;
            dataRow["LiczbaSztuk"] = trancheDomain.NumberOfItems;
            dataRow["RabatKwotowy"] = trancheDomain.QuotaDiscount;
            dataRow["CenaNetto"] = trancheDomain.PriceNetto;
            dataRow["CenaBrutto"] = trancheDomain.PriceBrutto;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }

        public List<Domain.Entity.Tranche> MapAllFrom(DataTable trancheTable)
        {
            List<Domain.Entity.Tranche> result = new List<Domain.Entity.Tranche>();
            foreach (DataRow trancheRow in trancheTable.Rows)
                result.Add(MapFrom(trancheTable,
                    trancheTable.Rows.IndexOf(trancheRow)));
            return result;
        }

        public Domain.Entity.Tranche MapFrom(DataTable trancheTable)
        {
            return MapFrom(trancheTable, 0);
        }

        Domain.Entity.Tranche MapFrom(DataTable trancheTable, int numberOfRow)
        {
            DataRow trancheRow = trancheTable.Rows[numberOfRow];
            var counterpartyStock = (new CounterpartysStockMapper(new DAL.InternalSysDAO.CounterpartysStock())).MapFrom(
               trancheDAO.GetCounterpartysStock(trancheRow));
            var percentageDiscounts = (new PercentageDiscountMapper(new DAL.InternalSysDAO.PercentageDiscount(), new DAL.InternalSysDAO.CounterpartysStock())).MapAllFrom(
               trancheDAO.GetPercentageDiscounts(trancheRow));
            return new Domain.Entity.Tranche(
                Convert.ToInt32(trancheRow["ID"]),
                counterpartyStock,
                Convert.ToInt32(trancheRow["LiczbaSztuk"]),
                Convert.ToInt32(trancheRow["ZamowienieID"]),
                Convert.ToDouble(trancheRow["RabatKwotowy"]),
                percentageDiscounts
                );
        }

    }
}
