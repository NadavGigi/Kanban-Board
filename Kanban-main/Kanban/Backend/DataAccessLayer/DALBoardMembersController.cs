using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DALBoardMembersController : DALController
    {
        private const string BoardMembersTableName = "BoardMembers";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DALBoardMembersController");
        public DALBoardMembersController() : base(BoardMembersTableName) { }
        /// <summary>
        /// insert a task to the database
        /// </summary>
        /// <param name="task">the task we want to insert</param>
        public bool Insert(DBoardMembers boardm)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardMembersTableName} ({DBoardMembers.boardMBoardID},{DBoardMembers.boardMEmail}) " +
                        $"VALUES (@boardId,@email);";

                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardId", boardm.boardId);
                    SQLiteParameter emailParam = new SQLiteParameter(@"email", boardm.email);


                    command.Parameters.Add(boardIDParam);
                    command.Parameters.Add(emailParam);

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
        /// deleting a board member from board from data base using boardId and email as keys
        /// </summary>
        /// <param name="boardId">board id of the board that contains the member</param>
        /// <param name="useremail">email of the user</param>
        public bool Delete(int boardId, string useremail)
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"delete from {BoardMembersTableName} where boardId={boardId} and email={useremail}"
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
        public bool Delete(int boardId)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"delete from {BoardMembersTableName} where boardId={boardId}"
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
        /// <summary>
        /// convert sql reader object to data access layer object
        /// </summary>
        /// <param name="reader">sql object</param>
        protected override DAL ConvertReaderToObject(SQLiteDataReader reader)
        {
            DBoardMembers result = new DBoardMembers(Convert.ToInt32(reader.GetValue(0)), reader.GetString(1));
            return result;
        }
        /// <summary>
        /// returns a list filled with all the board members
        /// </summary>
        public List<DBoardMembers> ListOfBoardMembers()
        {
            List<DBoardMembers> list1 = Select().Cast<DBoardMembers>().ToList();
            return list1;
        }
        /// <summary>
        /// returns a list filled with all the board members
        /// </summary>
        public List<string> Members(int boardid)
        {
            List<string> results = new List<string>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {BoardMembersTableName} where boardId={boardid};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(dataReader.GetString(1));
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
    }
}