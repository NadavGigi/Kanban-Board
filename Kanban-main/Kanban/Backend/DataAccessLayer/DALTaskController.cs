using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DALTaskController : DALController
    {
        private const string TaskTableName = "Task";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DALTaslkController");

        /// <summary>
        /// convert sql reader object to data access layer object
        /// </summary>
        /// <param name="reader">sql object</param>
        public DALTaskController() : base(TaskTableName) { }
        protected override DAL ConvertReaderToObject(SQLiteDataReader reader) // reading the data from database and create a new DataTask Object
        {
            DTask result = new DTask(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), reader.GetString(3), reader.GetString(4), reader.GetString(5), DateTime.Parse(reader.GetString(6)), reader.GetString(7));
            return result;
        }

        /// <summary>
        /// insert a task to the database
        /// </summary>
        /// <param name="task">the task we want to insert</param>
        public bool Insert(DTask task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({DTask.taskBoardId},{ DTask.taskId},{DTask.taskColumn},{DTask.taskTitle},{DTask.taskDescription},{DTask.taskDueDate},{DTask.taskCreationTime},{DTask.taskEmailAssignee})" +
                        $"VALUES (@boardID,@taskId,@column,@title,@description,@dueDate,@creationTime,@emailAssignee);";

                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardID", task.boardId);
                    SQLiteParameter taskIDParam = new SQLiteParameter(@"taskId", task.ID);
                    SQLiteParameter columnParam = new SQLiteParameter(@"column", task.column);
                    SQLiteParameter titleParam = new SQLiteParameter(@"title", task.title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"description", task.description);
                    SQLiteParameter DueDateParam = new SQLiteParameter(@"dueDate", task.dueDate);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTime", task.creationTime);
                    SQLiteParameter emailAssigneeParam = new SQLiteParameter(@"emailAssignee", task.emailAssignee);

                    command.Parameters.Add(boardIDParam);
                    command.Parameters.Add(taskIDParam);
                    command.Parameters.Add(columnParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(DueDateParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(emailAssigneeParam);
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
        /// updating int values in the database using boardId and key as keys
        /// </summary>
        /// <param name="boardId">board id of the board that contains the task</param>
        /// <param name="key">the task id</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(int boardId, int key, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"update {TaskTableName} set [{attributeName}]=@{attributeName} WHERE boardId={boardId} AND taskId={key}"
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
        /// updating string values in the database using boardId and key as keys
        /// </summary>
        /// <param name="boardId">board id of the board that contains the task</param>
        /// <param name="key">the task id</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(int boardId, int key, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"update {TaskTableName} set [{attributeName}]=@{attributeName} where boardId={boardId} AND taskId={key}"
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
        /// updating DateTime in the database using boardId and key as keys
        /// </summary>
        /// <param name="boardId">board id of the board that contains the task</param>
        /// <param name="key">the task id</param>
        /// <param name="attributeName">the name of the object we want to update </param>
        /// <param name="attributeValue">the value of the object we want to update</param>
        public bool Update(int boardId, int key, string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"update {TaskTableName} set [{attributeName}]=@{attributeName} where boardId={boardId} AND taskId={key}"
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
        /// deleting a task from data base using boardId as key
        /// </summary>
        /// <param name="boardId">board id of the board that contains the task</param>
        public bool Delete(int boardId)
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand {
                    Connection = connection,
                    CommandText = $"delete from {TaskTableName} where boardId={boardId}"
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
        /// returns a list filled with all the tasks
        /// </summary>
        public List<DTask> ListOfTasks()
        {
            List<DTask> list1 = Select().Cast<DTask>().ToList();
            return list1;
        }
    }
}