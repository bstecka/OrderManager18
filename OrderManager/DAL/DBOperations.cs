using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAO
{
    class DBOperations
    {
        private static SqlConnection connection;

        private DBOperations() { }
        
        public static SqlConnection Connection
        {
            get
            {
                if(connection == null)
                    connection = new SqlConnection("Server=PIOTRDELL; database=OrderManager; Trusted_Connection=yes");
                return connection;
            }
        }

        public static DataTable Select(string command)
        {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, Connection);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
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
