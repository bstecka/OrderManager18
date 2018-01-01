using OrderManager.Domain;
using OrderManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DTO
{
    class PriorityMapper : IMapperBase<List<Priority>>
    {
        public List<List<Priority>> MapAllFrom(DataTable tDAO)
        {
            throw new NotImplementedException();
        }

        public List<Priority> MapFrom(DataTable tDAO)
        {
            string field = tDAO.Rows[0]["ListaKryteriow"].ToString();
            string[] criteria = field.Split(new char[1] { ',' });
            List<Priority> priorities = new List<Priority>();
            foreach (string criterium in criteria)
                priorities.Add(GetPriority(criterium));
            return priorities;
        }

        public static Priority GetPriority(string priorityValue)
        {
            switch (priorityValue)
            {
                case "cena": return Priority.Price;
                case "odleglosc": return Priority.Distance;
                case "czestosc": return Priority.Frequency;
                default: throw new ArgumentException();
            }
        }

        public DataTable MapTo(List<Priority> tDomain)
        {
            throw new NotImplementedException();
        }
    }
}
