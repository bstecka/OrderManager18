using OrderManager.DAL.InternalSysDAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DTO
{

    class OrderMapper : IMapperBase<Domain.Entity.Order>
    {
        IOrderDAO orderDAO;

        public OrderMapper(IOrderDAO orderDAO)
        {
            this.orderDAO = orderDAO;
        }

        public List<Domain.Entity.Order> MapAllFrom(DataTable orderTable)
        {
            List<Domain.Entity.Order> result = new List<Domain.Entity.Order>();
            foreach (DataRow orderRow in orderTable.Rows)
                result.Add(MapFrom(orderTable,
                    orderTable.Rows.IndexOf(orderRow)));
            return result;
        }

        public Domain.Entity.Order MapFrom(DataTable orderTable)
        {
            return MapFrom(orderTable, 0);
        }

        Domain.Entity.Order MapFrom(DataTable orderTable, int numberOfRow)
        {
            DataRow orderRow = orderTable.Rows[numberOfRow];
            var counterparty = (new CounterpartyMapper()).MapFrom(
               orderDAO.GetCounterparty(orderRow));
            var orderStateRow = orderDAO.GetOrderState(orderRow).Rows[0];
            var tranches = (new TrancheMapper(new DAL.InternalSysDAO.Tranche())).MapFrom(
                orderDAO.GetTranches(orderRow));
            var creator = (new UserMapper()).MapFrom(
                orderDAO.GetUser(orderRow));
            return new Domain.Entity.Order(
                Convert.ToInt32(orderRow["ID"]),
                Convert.ToString(orderRow["nazwa"]),
                counterparty,
                Convert.ToDouble(orderRow["SumaWartosciPozycjiNetto"]),
                Convert.ToDouble(orderRow["SumaWartosciPozycjiBrutto"]),
                Convert.ToDateTime(orderRow["DataZlozenia"]),
                Convert.ToDateTime(orderRow["DataZakonczenia"]),
                Convert.ToString(orderStateRow["StanZamowienia"]),
                creator
                );
        }

        public DataTable MapTo(Domain.Entity.Order orderDomain)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("ZamowienieNadrzedne");
            dataTable.Columns.Add("UzytkownikID");
            dataTable.Columns.Add("KontrahentID");
            dataTable.Columns.Add("SumaWartosciPozycjiNetto");
            dataTable.Columns.Add("SumaWartosciPozycjiBrutto");
            dataTable.Columns.Add("DataZlozenia");
            dataTable.Columns.Add("DataZakonczenia");
            dataTable.Columns.Add("StanZamowieniaID");
            dataTable.Columns.Add("Nazwa");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ID"] = orderDomain.Id;
            dataRow["ZamowienieNadrzedne"] = 1; //////////////////////////TEMPORARY
            dataRow["UzytkownikID"] = orderDomain.Creator.Id;
            dataRow["KontrahentID"] = orderDomain.Counterparty.Id;
            dataRow["SumaWartosciPozycjiNetto"] = orderDomain.NettoSum;
            dataRow["SumaWartosciPozycjiBrutto"] = orderDomain.BruttoSum;
            dataRow["DataZlozenia"] = orderDomain.DateOfCreation;
            dataRow["DataZakonczenia"] = orderDomain.DateOfConclusion;
            dataRow["StanZamowieniaID"] = 1; /////////////////////////////TEMPORARY
            dataRow["Nazwa"] = orderDomain.Name;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }
    }
}
