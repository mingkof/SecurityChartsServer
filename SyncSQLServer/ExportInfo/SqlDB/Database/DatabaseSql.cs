using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace ServerDBExt.Database
{
    /// <summary>
    /// Class that encapsulates a Sql Database connections 
    /// and CRUD operations
    /// </summary>
    public class DatabaseSql : IDisposable, IDatabase
    {
        private SqlConnection _connection;
        //SqlDependency dep;
        /// <summary>
        /// Default constructor which uses the "DefaultConnection" connectionString
        /// </summary>
        public DatabaseSql()
            : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string name
        /// </summary>
        /// <param name="connectionStringName"></param>
        public DatabaseSql(string connstr)
        {
            try
            {
                _connection = new SqlConnection(connstr);
            }
            catch (Exception)
            {
            }
        }

        private void OnNotificaton(object src, SqlInfoMessageEventArgs arg)
        {
            //可以从arg.Details中获得通知的具体信息，比如变化数据的RowId
            //    DataTable dt = arg.Details;

            //    Console.WriteLine("Notification Received. " + DateTime.Now.ToLongTimeString() + "  Changed data(rowid): " + arg.Details.Rows[0]["rowid"].ToString() + Environment.NewLine);
        }


        /// <summary>
        /// Executes a non-query Sql statement
        /// </summary>
        /// <param name="commandText">The Sql query to execute</param>
        /// <param name="parameters">Optional parameters to pass to the query</param>
        /// <returns>The count of records affected by the Sql statement</returns>
        public int Execute(string commandText, IEnumerable parameters, bool FinallyClose )
        {
            int result;

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteNonQuery();
            }
            finally
            {
                if(FinallyClose)
                    EnsureConnectionClosed();
            }

            return result;
        }

        /// <summary>
        /// Executes a Sql query that returns a single scalar value as the result.
        /// </summary>
        /// <param name="commandText">The Sql query to execute</param>
        /// <param name="parameters">Optional parameters to pass to the query</param>
        /// <returns></returns>
        public object QueryValue(string commandText, IEnumerable parameters, bool FinallyClose)
        {
            object result;

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteScalar();
            }
            catch
            {
                return null;
            }
            finally
            {
                if (FinallyClose)
                    EnsureConnectionClosed();
            }

            return result;
        }

        /// <summary>
        /// Executes a SQL query that returns a list of rows as the result.
        /// </summary>
        /// <param name="commandText">The Sql query to execute</param>
        /// <param name="parameters">Parameters to pass to the Sql query</param>
        /// <returns>A list of a Dictionary of Key, values pairs representing the 
        /// ColumnName and corresponding value</returns>
        public List<Dictionary<string, string>> Query(string commandText, IEnumerable parameters, bool FinallyClose)
        {
            List<Dictionary<string, string>> rows;
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                using (var reader = command.ExecuteReader())
                {
                    rows = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.IsDBNull(i) ? null : reader.GetValue(i).ToString();
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                }
            }
            finally
            {
                if (FinallyClose)
                    EnsureConnectionClosed();
            }

            return rows;
        }

        public bool DoEnsureOpen(Action<string> CB = null)
        {
            return EnsureConnectionOpen(CB);
        }

        public void DoEnsureClose()
        {
            EnsureConnectionClosed();
        }

        /// <summary>
        /// Opens a connection if not open
        /// </summary>
        private bool EnsureConnectionOpen(Action<string> CB = null)
        {
            if (_connection == null)
                return false;

            var retries = 3;
            if (_connection.State == ConnectionState.Open)
            {
                return true;
            }
            while (retries >= 0 && _connection.State != ConnectionState.Open)
            {
                try
                {
                    _connection.Open();
                }
                catch (Exception e)
                {
                    if (CB != null)
                    {
                        CB(e.Message);
                    }
                }
                finally
                {
                    retries--;
                    Thread.Sleep(30);
                }
            }

            if (_connection.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Closes a connection if open
        /// </summary>
        private void EnsureConnectionClosed()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
            Dispose();
        }

        /// <summary>
        /// Creates a SqlCommand with the given parameters
        /// </summary>
        /// <param name="commandText">The Sql query to execute</param>
        /// <param name="parameters">Parameters to pass to the Sql query</param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string commandText, IEnumerable parameters)
        {
            var command = _connection.CreateCommand();
            //command.BindByName = true;
            command.CommandText = commandText;
            AddParameters(command, parameters);

            return command;
        }

        /// <summary>
        /// Adds the parameters to a Sql command
        /// </summary>
        /// <param name="command">The Sql query to execute</param>
        /// <param name="parameters">Parameters to pass to the Sql query</param>
        private static void AddParameters(SqlCommand command, IEnumerable parameters)
        {
            if (parameters == null) return;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Helper method to return query a string value 
        /// </summary>
        /// <param name="commandText">The Sql query to execute</param>
        /// <param name="parameters">Parameters to pass to the Sql query</param>
        /// <returns>The string value resulting from the query</returns>
        public string GetStrValue(string commandText, IEnumerable parameters, bool FinallyClose)
        {
            var value = QueryValue(commandText, parameters, FinallyClose) as string;
            return value;
        }

        public void Dispose()
        {
            if (_connection == null) return;

            _connection.Dispose();
            _connection = null;
        }
    }
}
