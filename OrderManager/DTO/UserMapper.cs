using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.Domain.Entity;

namespace OrderManager.DTO
{
    /// <summary>
    /// UNFINISHED
    /// </summary>
    class UserMapper : IMapperBase<Domain.Entity.User>
    {
        public List<User> MapAllFrom(DataTable userTable)
        {
            List<User> result = new List<User>();
            foreach (DataRow userRow in userTable.Rows)
                result.Add(MapFrom(userTable,
                    userTable.Rows.IndexOf(userRow)));
            return result;
        }

        public User MapFrom(DataTable userTable)
        {
            return MapFrom(userTable, 0);
        }

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
