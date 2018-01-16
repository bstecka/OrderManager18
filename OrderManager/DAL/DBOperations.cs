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

        /// <summary>
        /// Prevents a default instance of the <see cref="DBOperations"/> class from being created.
        /// </summary>
        private DBOperations() { }

        /// <summary>
        /// Estabilishes the connection with the MS SQL database.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public static SqlConnection Connection
        {
            get
            {
                if(connection == null)
                    connection = new SqlConnection("Server=PIOTRDELL; database=OrderManager3; Trusted_Connection=yes");
                return connection;
            }
        }

        /// <summary>
        /// Queries the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Returns the DataTable result of issuing a given command on the database.</returns>
        public static DataTable Query(string command)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, Connection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Inserts the specified entity inside the database.
        /// </summary>
        /// <param name="entityToSave">The entity to save.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Returns the id of the inserted row.</returns>
        public static int Insert(DataTable entityToSave, string tableName)
        {  
            connection.Open();
            int insertedRowId = insertWithoutCommit(entityToSave, tableName);
            connection.Close();
            return insertedRowId;
        }

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <param name="tableName">Name of the table.</param>
        public static void Update(DataTable entityToUpdate, string tableName)
        {
            connection.Open();
            updateWithoutCommit(entityToUpdate, tableName);
            connection.Close();
        }

        /// <summary>
        /// Updates the entity in the database without committing.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <param name="tableName">Name of the table.</param>
        private static void updateWithoutCommit(DataTable entityToUpdate, string tableName)
        {
            String commandText = "";
            foreach (DataRow row in entityToUpdate.Rows)
            {
                foreach (DataColumn column in entityToUpdate.Columns)
                {
                    if (!column.ColumnName.ToString().Equals("ID"))
                    {
                        commandText += column.ColumnName.ToString() + " = @" + column.ColumnName.ToString() + "A, ";
                    }
                }
                commandText = commandText.Remove(commandText.Trim().Length - 1);
                commandText = "UPDATE " + tableName + " SET " + commandText + " WHERE ID = " + row["ID"];
                SqlCommand command = new SqlCommand(commandText, connection);
                foreach (DataColumn column in entityToUpdate.Columns)
                {
                    if (!column.ColumnName.ToString().Equals("ID"))
                    {
                        command.Parameters.AddWithValue("@" + column.ColumnName.ToString() + "A", row[column.ColumnName.ToString()]);
                    }
                }
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Inserts a value inside database without committing.
        /// </summary>
        /// <param name="entityToSave">The entity to save.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Returns the id of the inserted row.</returns>
        private static int insertWithoutCommit(DataTable entityToSave, string tableName)
        {
            String commandText = "";
            String valuesText = "";
            int insertedRowId = 0;
            foreach (DataRow row in entityToSave.Rows)
            {
                foreach (DataColumn column in entityToSave.Columns)
                {
                    if (!column.ColumnName.ToString().Equals("ID"))
                    {
                        commandText += column.ColumnName.ToString() + ", ";
                    }
                }
                foreach (DataColumn column in entityToSave.Columns)
                {
                    if (!column.ColumnName.ToString().Equals("ID"))
                    {
                        valuesText += "@" + column.ColumnName.ToString() + "A, ";
                    }
                }
                commandText = commandText.Remove(commandText.Trim().Length - 1);
                valuesText = valuesText.Remove(valuesText.Trim().Length - 1);
                commandText = "INSERT INTO " + tableName + " (" + commandText + ") OUTPUT INSERTED.ID VALUES (" + valuesText + ")";
                SqlCommand command = new SqlCommand(commandText, connection);
                foreach (DataColumn column in entityToSave.Columns)
                {
                    if (!column.ColumnName.ToString().Equals("ID"))
                    {
                        command.Parameters.AddWithValue("@" + column.ColumnName.ToString() + "A", row[column.ColumnName.ToString()]);
                    }
                }
                insertedRowId = (int)command.ExecuteScalar();
            }
            return insertedRowId;
        }

        /// <summary>
        /// Updates an entity and commits.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        /// <param name="entityToSave">The entity to save.</param>
        /// <param name="tableName">Name of the table.</param>
        public static void UpdateAndSave(DataTable entityToUpdate, DataTable entityToSave, string tableName)
        {
            using(IDbTransaction tran = connection.BeginTransaction())
            {
                try
                {
                    insertWithoutCommit(entityToSave, tableName);
                    updateWithoutCommit(entityToUpdate, tableName);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Opens the connection
        /// </summary>
        /// <param name="command">The command.</param>
        public static void OpertionThatRequiresOpeningDBConnection(string command)
        {
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(command, Connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// Opens the connection and adds a dictionary of connection parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
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
