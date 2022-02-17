using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DALUserController : DALController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DALUserController");
        private const string tableName = "User";
        public DALUserController() : base(tableName)
        {

        }
        /// <summary>
        /// updating string values in the database using email as key
        /// </summary>
        /// <param name="email">email of the person who conducts the change.</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(string email, string attributeName, string attributeValue)
        {
            email = "'" + email + "'";
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{attributeName}]=@{attributeName} where email={email}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
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

        /// <summary>
        /// insert a row to the database
        /// </summary>
        /// <param name="user">the object we want to insert</param>

        public bool Insert(DUser user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {tableName} ({DUser.userEmail},{ DUser.userPassword})" +
                        $"VALUES (@email,@password);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"email", user.email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"password", user.password);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Warn("Insert failed");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        /// <summary>
        /// convert sql reader object to data access layer object
        /// </summary>
        /// <param name="reader">sql object</param>
        protected override DAL ConvertReaderToObject(SQLiteDataReader reader)
        {
            DUser result = new DUser(reader.GetString(0), reader.GetString(1));
            return result;
        }
        /// <summary>
        /// returns a list filled with all the users
        /// </summary>
        public List<DUser> ListOfUsers()
        {
            List<DUser> list1 = Select().Cast<DUser>().ToList();
            return list1;
        }

    }
}