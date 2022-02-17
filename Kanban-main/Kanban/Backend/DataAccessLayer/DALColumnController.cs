using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DALColumnController : DALController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DALColumnController");
        private const string ColumnTableName = "Column";
        public DALColumnController() : base(ColumnTableName)
        {

        }
        /// <summary>
        /// updating values in the database using boardId and columnOrdinal as keys
        /// </summary>
        /// <param name="boardId">board id of the board that contains the column</param>
        /// <param name="columnOrdinal">the columnOrdinal of the column</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(int boardId, int columnOrdinal, string attributeName, int attributeValue)
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {

                SQLiteCommand command = new SQLiteCommand() {

                    Connection = connection,
                    CommandText = $"UPDATE {ColumnTableName} SET [{attributeName}]=@{attributeName} WHERE columnOrdinal={columnOrdinal} AND  boardId ={boardId} "
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
        //updating string values
        public bool Update(int boardId, int columnOrdinal, string attributeName, string attributeValue)
        {

            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {

                SQLiteCommand command = new SQLiteCommand() {

                    Connection = connection,
                    CommandText = $"UPDATE {ColumnTableName} SET [{attributeName}]=@{attributeName} WHERE columnOrdinal={columnOrdinal} AND  boardId ={boardId} "
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
        /// returns a list filled with all the columns
        /// </summary>
        public List<DColumn> SelectAllColumns()
        {
            List<DColumn> result = Select().Cast<DColumn>().ToList();

            return result;
        }
        /// <summary>
        /// insert a column to the database
        /// </summary>
        /// <param name="column">the column we want to insert</param>
        public bool Insert(DColumn column)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({DColumn.columnBoardId},{DColumn.columnOrdinal},{DColumn.columnLimit},{DColumn.columnSize},{DColumn.columnName})" +
                        $"VALUES (@boardId,@columnOrdinal,@lim,@size,@name);";

                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", column.boardId);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"columnOrdinal", column.ordinal);
                    SQLiteParameter limParam = new SQLiteParameter(@"lim", column.lim);
                    SQLiteParameter SizeParam = new SQLiteParameter(@"size", column.sizeOfColumn);
                    SQLiteParameter nameParam = new SQLiteParameter(@"name", column.name);

                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(limParam);
                    command.Parameters.Add(SizeParam);
                    command.Parameters.Add(nameParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Warn(e.ToString());
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
            DColumn result = new DColumn(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3)), reader.GetString(4));
            return result;

        }
        /// <summary>
        /// deleting a column from data base using boardId as key
        /// </summary>
        /// <param name="boardId">board id of the board that contains the column</param>
        public bool Delete(int boardId,int columnOrdinal)
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"delete from {ColumnTableName} where boardId={boardId} AND columnOrdinal={columnOrdinal} "
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
                    CommandText = $"delete from {ColumnTableName} where boardId={boardId}  "
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
        /// returns a list filled with all the columns
        /// </summary>
        public List<DColumn> ListOfColumns()
        {
            List<DColumn> list1 = Select().Cast<DColumn>().ToList();
            return list1;
        }
    }


}
