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
        /// <summary>
        /// Unneeded mapper.
        /// </summary>
        /// <param name="tDAO">The t DAO.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<List<Priority>> MapAllFrom(DataTable tDAO)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps from the dataTable to a priority entity.
        /// </summary>
        /// <param name="priorityTable">The priority table.</param>
        /// <returns>Returns the priority entity.</returns>
        public List<Priority> MapFrom(DataTable priorityTable)
        {
            string field = priorityTable.Rows[0]["ListaKryteriow"].ToString();
            string[] criteria = field.Split(new char[1] { ',' });
            List<Priority> priorities = new List<Priority>();
            foreach (string criterium in criteria)
                priorities.Add(GetPriority(criterium));
            return priorities;
        }

        /// <summary>
        /// Maps a string value to Priority enum value.
        /// </summary>
        /// <param name="priorityValue">The priority value in a form readable to the end user.</param>
        /// <returns>Returns a value of the priority enum.</returns>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Unneeded mapper
        /// </summary>
        /// <param name="tDomain">The t domain.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public DataTable MapTo(List<Priority> tDomain)
        {
            throw new NotImplementedException();
        }
    }
}
