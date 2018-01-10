using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAO
{
    public class DBOperations
    {
        private static SqlConnection connection;

        private DBOperations() { }
        
        public static SqlConnection Connection
        {
            get
            {
                if(connection == null)
                    connection = new SqlConnection("Server=.\\SQLEXPRESS; database=OrderManager3; Trusted_Connection=yes");
                return connection;
            }
        }

        public static DataTable Query(string command)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, Connection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public static void Update(DataTable entity, string tableName)
        {
            connection.Open();
            String commandText = "";
            foreach (DataRow row in entity.Rows)
            {              
                foreach (DataColumn column in entity.Columns)
                {
                    if (!column.ColumnName.ToString().Equals("ID"))
                    {
                        commandText += column.ColumnName.ToString() + " = @" + column.ColumnName.ToString() + "A, ";
                    }
                }
                commandText = commandText.Remove(commandText.Trim().Length - 1);
                commandText = "UPDATE " + tableName + " SET " + commandText + " WHERE ID = " + row["ID"];
                SqlCommand command = new SqlCommand(commandText, connection);
                foreach (DataColumn column in entity.Columns)
                {
                    if (!column.ColumnName.ToString().Equals("ID"))
                    {
                        command.Parameters.AddWithValue("@" + column.ColumnName.ToString() + "A", row[column.ColumnName.ToString()]);
                    }
                }
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        public static void OpertionThatRequiresOpeningDBConnection(string command)
        {
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(command, Connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }

        public static void OpertionThatRequiresOpeningDBConnection(string command, Dictionary<string, string> parameters)
        {
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(command, Connection);
            foreach (KeyValuePair<string, string> parameter in parameters)
                sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }

    }
}
