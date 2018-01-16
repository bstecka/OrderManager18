using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.Domain.Entity;

namespace OrderManager.DTO
{
    class UserMapper : IMapperBase<Domain.Entity.User>
    {
        /// <summary>
        /// Maps from the dataTable to a list of user entities.
        /// </summary>
        /// <param name="userTable">The user DataTable.</param>
        /// <returns>Returns a list of user entities.</returns>
        public List<User> MapAllFrom(DataTable userTable)
        {
            List<User> result = new List<User>();
            foreach (DataRow userRow in userTable.Rows)
                result.Add(MapFrom(userTable,
                    userTable.Rows.IndexOf(userRow)));
            return result;
        }

        /// <summary>
        /// Maps from the dataTable to a user entity.
        /// </summary>
        /// <param name="userTable">The user table.</param>
        /// <returns>Returns a user entity.</returns>
        public User MapFrom(DataTable userTable)
        {
            return MapFrom(userTable, 0);
        }

        /// Maps from a specified row of the dataTable to a user entity.
        /// </summary>
        /// <param name="userTable">The user table.</param>
        /// <param name="numberOfRow">The number of row.</param>
        /// <returns>Returns the user entity.</returns>
        User MapFrom(DataTable userTable, int numberOfRow)
        {
            DataRow userRow = userTable.Rows[numberOfRow];
            return new User(
                Convert.ToInt32(userRow["ID"]),
                Convert.ToString(userRow["Imie"]),
                Convert.ToString(userRow["Nazwisko"]),
                Convert.ToString(userRow["Login"]),
                Convert.ToString(userRow["Haslo"]),
                Convert.ToString(userRow["NrTelefonu"]),
                Convert.ToString(userRow["Typ"])
                );
        }

        /// <summary>
        /// Maps from the user entity to a DataTable.
        /// </summary>
        /// <param name="userDomain">The user enity.</param>
        /// <returns>Returns a DataTable with values corresponding to the user entity.</returns>
        public DataTable MapTo(User userDomain)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Imie");
            dataTable.Columns.Add("Nazwisko");
            dataTable.Columns.Add("Login");
            dataTable.Columns.Add("Haslo");
            dataTable.Columns.Add("NrTelefonu");
            dataTable.Columns.Add("Typ");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ID"] = userDomain.Id;
            dataRow["Imie"] = userDomain.Name;
            dataRow["Nazwisko"] = userDomain.Surname;
            dataRow["Login"] = userDomain.Login;
            dataRow["Haslo"] = userDomain.Password;
            dataRow["NrTelefonu"] = userDomain.PhoneNumber;
            dataRow["Typ"] = userDomain.Type;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }
    }
}
