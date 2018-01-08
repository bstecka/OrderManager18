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
            var tranches = (new TrancheMapper(new DAL.InternalSysDAO.TrancheDAO())).MapAllFrom(
                orderDAO.GetTranches(orderRow));
            var creator = (new UserMapper()).MapFrom(
                orderDAO.GetUser(orderRow));
            if (DBNull.Value.Equals(orderRow["DataZakonczenia"]))
                return new Domain.Entity.Order(
                    Convert.ToInt32(orderRow["ID"]),
                    Convert.ToString(orderRow["nazwa"]),
                    counterparty,
                    Convert.ToDateTime(orderRow["DataZlozenia"]),
                    (ORDERSTATE)Enum.Parse(typeof(ORDERSTATE),
                    orderRow["StanZamowieniaID"].ToString()),
                    creator,
                    tranches
                );
            else
                return new Domain.Entity.Order(
                    Convert.ToInt32(orderRow["ID"]),
                    Convert.ToString(orderRow["nazwa"]),
                    counterparty,
                    Convert.ToDateTime(orderRow["DataZlozenia"]),
                    Convert.ToDateTime(orderRow["DataZakonczenia"]),
                    (ORDERSTATE)Enum.Parse(typeof(ORDERSTATE),
                    orderRow["StanZamowieniaID"].ToString()),
                    creator,
                    tranches
                );
        }

        public DataTable MapTo(Domain.Entity.Order orderDomain)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("ZamowienieNadrzedne");
            dataTable.Columns.Add("UzytkownikID");
            dataTable.Columns.Add("KontrahentID");
            dataTable.Columns.Add("DataZlozenia");
            dataTable.Columns.Add("DataZakonczenia");
            dataTable.Columns.Add("StanZamowieniaID");
            dataTable.Columns.Add("Nazwa");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ID"] = orderDomain.Id;
            dataRow["ZamowienieNadrzedne"] = orderDomain.ParentOrder;
            dataRow["UzytkownikID"] = orderDomain.Creator.Id;
            dataRow["KontrahentID"] = orderDomain.Counterparty.Id;
            dataRow["DataZlozenia"] = orderDomain.DateOfCreation;
            dataRow["DataZakonczenia"] = orderDomain.DateOfConclusion;
            dataRow["StanZamowieniaID"] = (int) orderDomain.State;
            dataRow["Nazwa"] = orderDomain.Name;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }
    }
}
