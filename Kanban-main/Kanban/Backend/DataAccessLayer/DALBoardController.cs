using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DALBoardController : DALController
    {
        private const string BoardTableName = "Board";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DALBoardController");
        public DALBoardController() : base(BoardTableName) { }
        /// <summary>
        /// updating int values in the database using boardId as key
        /// </summary>
        /// <param name="boardId">board id of the board that contains the task</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(int boardId, string attributeName, int attributeValue)
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"update {BoardTableName} set [{attributeName}]=@{attributeName} where boardId={boardId}"
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
        /// updating string values in the database using boardId as key
        /// </summary>
        /// <param name="boardId">board id of the board that contains the task</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(int boardId, string attributeName, string attributeValue)
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"update {BoardTableName} set [{attributeName}]=@{attributeName} where boardId={boardId}"
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
        /// insert a board to the database
        /// </summary>
        /// <param name="board">the board we want to insert</param>
        public bool Insert(DBoard board) // insert new board to database according to the dataBoard fields
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({DBoard.boardBoardId},{DBoard.boardEmailOfCreator},{DBoard.boardName},{DBoard.boardTaskID},{DBoard.boardcolumnCounter}) " +
                        $"VALUES (@BoardID,@emailOfCreator,@name,@taskID,@columnID);";

                    SQLiteParameter boardIDParam = new SQLiteParameter(@"BoardID", board.boardId);
                    SQLiteParameter emailOfCreatorParam = new SQLiteParameter(@"emailOfCreator", board.emailOfCreator);
                    SQLiteParameter nameParam = new SQLiteParameter(@"name", board.name);
                    SQLiteParameter taskIDParam = new SQLiteParameter(@"taskID", board.taskID);
                    SQLiteParameter columnIDParam = new SQLiteParameter(@"columnID", board.columnID);

                    command.Parameters.Add(emailOfCreatorParam);
                    command.Parameters.Add(taskIDParam);
                    command.Parameters.Add(boardIDParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(columnIDParam);
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
        /// deleting a board from data base using boardId as key
        /// </summary>
        /// <param name="boardId">board id of the board</param>
        public bool Delete(int boardId)
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"delete from {BoardTableName} where boardId={boardId}"
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
            DALColumnController DCC = new DALColumnController();
            DALBoardMembersController DBMC = new DALBoardMembersController();
            DALTaskController DTC = new DALTaskController();
            DCC.Delete(boardId);
            DBMC.Delete(boardId);
            DTC.Delete(boardId);
            return res > 0;
        }
        /// <summary>
        /// convert sql reader object to data access layer object
        /// </summary>
        /// <param name="reader">sql object</param>
        protected override DAL ConvertReaderToObject(SQLiteDataReader reader)
        {
            DBoard result = new DBoard(Convert.ToInt32(reader.GetValue(0)), reader.GetString(1), reader.GetString(2), Convert.ToInt32(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4)));
            return result;
        }
        /// <summary>
        /// returns a list filled with all the boards
        /// </summary>
        public List<DBoard> ListOfBoards()
        {
            List<DBoard> list1 = Select().Cast<DBoard>().ToList();
            return list1;
        }
    }
}