using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.Domain.Entity;
using OrderManager.DAL.InternalSysDAO;
using System.Data;

namespace OrderManager.DTO
{
    public class CounterpartyMapper : IMapperBase<Counterparty>
    {
        public Domain.Entity.Counterparty MapFrom(DataTable counterpartiesDAO)
        {
            if (counterpartiesDAO.Rows.Count != 1)
                throw new ArgumentOutOfRangeException();
            return MapFrom(counterpartiesDAO, 0);
        }

        private Domain.Entity.Counterparty MapFrom(DataTable counterpartiesDAO, int numberOfRow)
        {
            DataRow counterpartyRow = counterpartiesDAO.Rows[numberOfRow];
            Domain.Entity.Counterparty counterparty = new Domain.Entity.Counterparty(
            Convert.ToInt32(counterpartyRow["ID"]),
            Convert.ToInt64(counterpartyRow["NIP"]),
            Convert.ToString(counterpartyRow["Nazwa"]),
            Convert.ToInt32(counterpartyRow["Odleglosc"]));
            return counterparty;
        }

        public DataTable MapTo(Domain.Entity.Counterparty counterpartyDomain)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("NIP");
            dataTable.Columns.Add("Nazwa");
            dataTable.Columns.Add("Odleglosc");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ID"] = counterpartyDomain.Id;
            dataRow["NIP"] = counterpartyDomain.Nip;
            dataRow["Nazwa"] = counterpartyDomain.Name;
            dataRow["Odleglosc"] = counterpartyDomain.Distance;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }

        public List<Counterparty> MapAllFrom(DataTable counterpartyDAO)
        {
            List<Counterparty> result = new List<Counterparty>();
            foreach (DataRow counterpartyRow in counterpartyDAO.Rows)
                result.Add(MapFrom(counterpartyDAO,
                    counterpartyDAO.Rows.IndexOf(counterpartyRow)));
            return result;
        }

    }
}
