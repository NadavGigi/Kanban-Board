using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DALController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DALController");
        protected readonly string _connectionString;
        private readonly string _tableName;
        public DALController(string tableName)
        {
            /// <summary>
            /// defining the path to database in an abstract way
            /// </summary>
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }
        /// <summary>
        /// updating int values in the database using id key
        /// </summary>
        /// <param name="id">id which will be used as key to find specific data</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(long id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Warn("update faild");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        /// <summary>
        /// updating string values in the database using id key
        /// </summary>
        /// <param name="id">id which will be used as key to find specific data</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(long id, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }
        /// <summary>
        /// select values from the database
        /// </summary>
        /// <param name="reader">sql object</param>
        protected List<DAL> Select()
        {
            List<DAL> results = new List<DAL>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} ;";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }
        /// <summary>
        /// convert sql reader object to data access layer object
        /// </summary>
        /// <param name="reader">sql object</param>
        protected abstract DAL ConvertReaderToObject(SQLiteDataReader reader);
        /// <summary>
        /// delete all values from a table
        /// </summary>
        public bool DeleteAll()
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} "
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }


    }
}