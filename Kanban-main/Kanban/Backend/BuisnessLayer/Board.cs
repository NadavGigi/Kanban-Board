using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Board
    {
        private string name;
        private string creatorEmail;//email of creator
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BuisnessLayer.Board");
        private int taskID;
        private int boardID;
        private List<string> boardMembers;
        private int columnCounter;//holds the hights key in the column list
        private Dictionary<int, Column> allColumns;//holds up all columns with key=column ordinal and not ID
        //constructor
        public Board(string creatorEmail, string name, int boardID)
        {
            this.name = name;
            this.creatorEmail = creatorEmail;
            this.allColumns = new Dictionary<int, Column>();
            this.allColumns[0] = new Column(boardID,0,"backlog");
            this.allColumns[1] = new Column(boardID, 1, "in progress");
            this.allColumns[2] = new Column(boardID, 2, "done");
            this.boardID = boardID;
            this.columnCounter = 3;
            boardMembers = new List<string>();
            boardMembers.Add(creatorEmail);
            this.taskID = 0;//no tasks has been added
            //adding a new board to the data base
            DataAccessLayer.DALBoardController DBC = new DataAccessLayer.DALBoardController();
            DBC.Insert(this.toDalObject());
            DataAccessLayer.DALBoardMembersController DBMC = new DataAccessLayer.DALBoardMembersController();
            DataAccessLayer.DBoardMembers toInsert = new DataAccessLayer.DBoardMembers(boardID, creatorEmail);
            DBMC.Insert(toInsert);
        }
        public Board(DataAccessLayer.DBoard dboard, List<string> members, Dictionary<int, Column> columns)
        {
            this.name = dboard.name;
            this.creatorEmail = dboard.emailOfCreator;
            this.allColumns = columns;
            this.boardID = dboard.boardId;
            this.boardMembers = members;
            this.taskID = dboard.taskID;
            this.columnCounter = columns.Count();
        }
        public List<string> getBoardMembers() { return boardMembers; }
        public int getBoardID() { return boardID; }
        /// <summary>
        /// return the name of the board
        /// </summary>
        /// <returns>string of board name</returns>
        public string GetBoardName() { return name; }
        /// <summary>
        /// Get Email of creator
        /// </summary>
        /// <returns>string - email of the creator</returns>
        public int GetTaskId() { return taskID; }
        /// <summary>
        /// Get taskID
        /// </summary>
        /// <returns>string - email of the creator</returns>
        public string GetEmailofcreator() { return creatorEmail; }
        /// <summary>
        /// create and return a list of tasks of specific column ordinal
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>list of tasks</returns>

        public Column GetColumn(int columnOrdinal)
        {
            return allColumns[columnOrdinal];
        }
        /// <summary>
        /// get column name specific column ordinal
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is not found</exception>
        public string getColumnName(int columnOrdinal)
        {
            if (!allColumns.ContainsKey(columnOrdinal))
            {
                log.Warn("Column ordinal not found");
                throw new Exception("Column ordinal not found");
            }
            return allColumns[columnOrdinal].getName();
        }
        /// <summary>
        /// limiting the column size.set a max nuber of tasks for the column
        /// </summary>
        /// <param name="newlim">new limit</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is diffrent from 0,1,2</exception>
        public void LimitColumn(string userEmail, int columnOrdinal, int newlim)//limiting the column by coulumn ordinal
        {
            isMember(userEmail);
            if (!allColumns.ContainsKey(columnOrdinal))
            {
                log.Warn("Column ordinal not found");
                throw new Exception("Column ordinal not found");
            }
            allColumns[columnOrdinal].SetLimit(newlim);
        }
        /// <summary>
        /// get the limit of a column
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>int-the limit</returns>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is diffrent from 0,1,2</exception>
        public int getColumnLimit(int columnOrdinal)
        {
            if (!allColumns.ContainsKey(columnOrdinal))
            {
                log.Warn("Column ordinal not found");
                throw new Exception("Column ordinal not found");
            }
            return allColumns[columnOrdinal].getLim();
        }
        /// <summary>
        /// get a taskusing its id
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>a task after finding it</returns>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is diffrent from 0,1,2</exception>
        public Task GetTask(int columnOrdinal, int taskId)
        {
            if (!allColumns.ContainsKey(columnOrdinal))
            {
                log.Warn("Column ordinal not found");
                throw new Exception("Column ordinal not found");
            }
            return allColumns[columnOrdinal].getTask(taskId);//check if the task is in the column    
        }
        /// <summary>
        /// advancong a task only if its not done(its in in progress or backlog)
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is diffrent from 0,1</exception>
        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            if (allColumns[columnOrdinal].getTask(taskId).getEmailAssignee() != email)
            {
                log.Warn("Only the task assignee can advance this task");
                throw new Exception("Only the task assignee can advance this task");
            }
            if (!allColumns.ContainsKey(columnOrdinal))
            {
                log.Warn("Column ordinal not found");
                throw new Exception("Column ordinal not found");
            }
            if (columnOrdinal == columnCounter)
            {
                log.Warn("cant advance a task in done column");
                throw new Exception("cant advance a task in done column");
            }
            Task task = allColumns[columnOrdinal].getTask(taskId);
            allColumns[columnOrdinal].RemoveTask(taskId);
            allColumns[columnOrdinal+1].Add(task);  
            task.setColumnOrdinal(columnOrdinal+1);
        }
        /// <summary>
        /// adding a new task to backlog,if its exceeding the limit it will thorw exeption
        /// </summary>
        /// <param name="title">title of the task</param>
        /// <param name="description">description of the task</param>
        /// <param name="duedate">deo date of the task</param>
        /// <returns></returns>
        public Task AddTask(string title, string description, DateTime duedate, string emailAssignee)
        {
            isMember(emailAssignee);
            Task task = new Task(boardID, 0, title, description, duedate, taskID, emailAssignee);
            taskID++;
            allColumns[0].Add(task);
            //data base
            DataAccessLayer.DALBoardController DBC = new DataAccessLayer.DALBoardController();
            DBC.Update(boardID, "taskId", taskID);
            return task;
        }
        /// <summary>
        /// updating task due date
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">new deo date of the task</param>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is diffrent from 0,1</exception>
        public void UpdateTaskDueDate(string userEmail, int columnOrdinal, int taskId, DateTime dueDate)
        {
            isMember(userEmail);
            if (!allColumns.ContainsKey(columnOrdinal))
            {
                log.Warn("Column ordinal not found");
                throw new Exception("Column ordinal not found");
            }
            else if (columnOrdinal == columnCounter)
            {
                log.Warn("Can not update task that is alreay done");
                throw new Exception("Can not update task that is alreay done");
            }
            allColumns[columnOrdinal].getTask(taskId).changeDueDate(userEmail, dueDate);

        }
        /// <summary>
        /// updating task title
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">title of the task</param>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is diffrent from 0,1</exception>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            isMember(email);
            if (!allColumns.ContainsKey(columnOrdinal))
            {
                log.Warn("Column ordinal not found");
                throw new Exception("Column ordinal not found");
            }
            else if (columnOrdinal == columnCounter)
            {
                log.Warn("Can not update task that is alreay done");
                throw new Exception("Can not update task that is alreay done");
            }
            allColumns[columnOrdinal].getTask(taskId).changeTitle(email, title);
        }
        /// <summary>
        /// updating task description
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">title of the task</param>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is diffrent from 0,1</exception>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            isMember(email);
            if (!allColumns.ContainsKey(columnOrdinal))
            {
                log.Warn("Column ordinal not found");
                throw new Exception("Column ordinal not found");
            }
            else if (columnOrdinal == columnCounter)
            {
                log.Warn("Can not update task that is alreay done");
                throw new Exception("Can not update task that is alreay done");
            }
            allColumns[columnOrdinal].getTask(taskId).changeDescription(email,description);
        }
        /// <summary>
        /// check if the email is a member of the board
        /// </summary>
        /// <param name="email"></param> the email that we want to check
        /// <returns></returns>
        private bool isMember(string email)
        {
            if (name == "email")
                return true;
            if (boardMembers.Contains(email))
                return true;
            else
            {
                log.Warn("The User is not a Board Member");
                throw new Exception("The User is not a Board Member");
            }
        }
        /// <summary>
        /// join a new member to the board
        /// </summary>
        /// <param name="userEmail"></param>the email that we want to add as a member
        public void JoinBoard(string userEmail)
        {
            if (boardMembers.Contains(userEmail))
            {
                log.Warn("user is already a member");
                throw new Exception("user is already a member");
            }
            boardMembers.Add(userEmail);
            //database
            DataAccessLayer.DALBoardMembersController DBMC = new DataAccessLayer.DALBoardMembersController();
            DataAccessLayer.DBoardMembers toInsert = new DataAccessLayer.DBoardMembers(boardID, userEmail);
            DBMC.Insert(toInsert);
        }
        public void LeaveBoard(string userEmail)
        {
            if (!boardMembers.Contains(userEmail))
                throw new Exception("the user is not a board member");
            boardMembers.Remove(userEmail);
            //data base
            DataAccessLayer.DALBoardMembersController DBMC = new DataAccessLayer.DALBoardMembersController();
            DBMC.Delete(boardID, userEmail);
        }
        /// <summary>
        /// assign a task to a specific user
        /// </summary>
        /// <param name="userEmail"></param>the user that want to assign the task
        /// <param name="columnOrdinal"></param>the column ordinal that the task in
        /// <param name="taskId"></param> the task ID
        /// <param name="emailAssignee"></param> the email that we want to assign the task to
        public void AssignTask(string userEmail, int columnOrdinal, int taskId, string emailAssignee)
        {
            isMember(userEmail);
            isMember(emailAssignee);
            GetTask(columnOrdinal, taskId).setEmailAssignee(emailAssignee);
        }
        /// <summary>
        /// returns all the task a specific user is assigned to in the board
        /// </summary>
        /// <param name="email"></param>the email that we want to check which task is assigned to
        /// <returns></returns>
        public List<Task> getAssignTasks(string email)
        {
            List<Task> usertasks = new List<Task>();
            foreach (var column in allColumns.Values)
            {
                if (column.getColumnOrdinal()!=0&& column.getColumnOrdinal()!=columnCounter) {
                    foreach (var task in column.GetTaskList())
                    {
                        if (task.getEmailAssignee() == email)
                        {
                            usertasks.Add(task);
                        }
                    }
                }
            }
            return usertasks;
        }
        /// <summary>
        /// convert business object to dal object
        /// </summary>
        /// <returns></returns>
        public DataAccessLayer.DBoard toDalObject()
        {
            DataAccessLayer.DBoard toReturn = new DataAccessLayer.DBoard(this.boardID, this.creatorEmail, this.name, this.taskID,this.columnCounter);
            log.Debug("Converted Board to DAL Object succesfully");
            return toReturn;
        }
        /// <summary>
        /// adding a column
        /// </summary>
        /// <param name="columnOrdinal"></param>the column ordinal that we want to add the board
        /// <param name="columnName"></param>the new column name
        /// <param name="userEmail"></param>the user that adding the column
        public void AddColumn(int columnOrdinal, string columnName,string userEmail)
        {
            isMember(userEmail);
            if(columnName == null)
            {
                log.Warn("Column most have name");
                throw new Exception("Column most have name");
            }
            if (columnOrdinal>columnCounter | columnOrdinal < 0)
            {
                log.Warn("wrong column ordinal");
                throw new Exception("wrong column ordinal");
            }
            if (columnCounter == columnOrdinal)//adding the column to the end of the dic
            {
                allColumns[columnOrdinal] = new Column(boardID, columnOrdinal, columnName);
            }
            for(int i=columnCounter-1;i>= columnOrdinal;i--)
            {
                GetColumn(i).setColumnOrdinal(i + 1);
                allColumns[i + 1] = allColumns[i];
            }
            allColumns[columnOrdinal] = new Column(boardID, columnOrdinal, columnName);
            DataAccessLayer.DALBoardController BC = new DataAccessLayer.DALBoardController();
            columnCounter++;
            BC.Update(boardID, "columnID", columnCounter);
            
        }
        /// <summary>
        /// renaming an exicting column
        /// </summary>
        /// <param name="userEmail"></param>the email that wants to rename
        /// <param name="columnOrdinal"></param>the column ordina that we want to rename
        /// <param name="newColumnName"></param>the new column name
        public void RenameColumn(string userEmail,int columnOrdinal, string newColumnName)
        {
            isMember(userEmail);
            GetColumn(columnOrdinal).setName(newColumnName);
        }
        /// <summary>
        /// moving the column according to the shift size
        /// </summary>
        /// <param name="userEmail"></param>the user that want to move the cilumn   
        /// <param name="columnOrdinal"></param>the column ordinak that the column is at the begining
        /// <param name="shiftSize"></param>the number of moves we want to move the column
        public void MoveColumn(string userEmail, int columnOrdinal, int shiftSize)
        {
            isMember(userEmail);
            int key = columnOrdinal + shiftSize;
            if (key < 0 || key >= columnCounter)
            {
                log.Warn("invalid shift size");
                throw new Exception("invalid shift size");
            }
            if (allColumns[columnOrdinal].getColumnSize() > 0)
            {
                log.Warn("Can not move unempty column");
                throw new Exception("Can not move unempty column");
            }
            Column newColumn = allColumns[columnOrdinal];
            DeleteColumn(columnOrdinal);
            AddColumn(key, newColumn.getName(), userEmail);
        }
        /// <summary>
        /// private methid which in use with the move column function
        /// </summary>
        /// <param name="columnOrdinal"></param>
        private void DeleteColumn(int columnOrdinal)
        {
            if (columnCounter == 2)
            {
                throw new Exception("cant delete a column when have only 2 left");
            }
            if (columnOrdinal >= columnCounter)
            {
                throw new Exception("column ordinal not found");
            }
            DataAccessLayer.DALColumnController DCC = new DataAccessLayer.DALColumnController();
            DCC.Delete(boardID, columnOrdinal);
            allColumns.Remove(columnOrdinal);
            columnCounter--;
            for (int i = columnOrdinal; i < columnCounter; i++)
            {
                allColumns[i] = allColumns[i + 1];
                allColumns.Remove(i + 1);
                allColumns[i].setColumnOrdinal(i);
            }
            DataAccessLayer.DALBoardController BC = new DataAccessLayer.DALBoardController();
            BC.Update(boardID, "columnID", columnCounter);
        }
        /// <summary>
        /// remove an exicisting column
        /// </summary>
        /// <param name="columnOrdinal"></param>the column ordinal
        /// <param name="userEmail"></param>the user that ewant to remove the column
        public void RemoveColumn(int columnOrdinal, string userEmail)
        {
            isMember(userEmail);
            if (columnCounter == 2)
            {
                log.Warn("cant delete a column when have only 2 left");
                throw new Exception("cant delete a column when have only 2 left");
            }
            if (columnOrdinal >= columnCounter)
            {
                log.Warn("column ordinal not found");
                throw new Exception("column ordinal not found");
            }
            DataAccessLayer.DALColumnController DCC = new DataAccessLayer.DALColumnController();
            DCC.Delete(boardID, columnOrdinal);
            if (columnOrdinal == 0)
            {
                allColumns[1].addTasksList(allColumns[0].GetTaskList());
            }
            else
            {
                allColumns[columnOrdinal - 1].addTasksList(allColumns[columnOrdinal].GetTaskList());
            }
            allColumns.Remove(columnOrdinal);
            columnCounter--;
            for (int i = columnOrdinal; i < columnCounter; i++)
            {
                allColumns[i] = allColumns[i + 1];
                allColumns.Remove(i + 1);
                allColumns[i].setColumnOrdinal(i);
            }
            DataAccessLayer.DALBoardController BC = new DataAccessLayer.DALBoardController();
            BC.Update(boardID, "columnID", columnCounter);
        }
        public List<Column> getAllColumns()
        {
            return allColumns.Values.ToList() ;
        }
    }
}