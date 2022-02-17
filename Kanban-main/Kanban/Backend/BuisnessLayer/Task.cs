using System;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Task
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.Task");
        private Validator validator = new Validator();
        private string title;
        private string description;
        private DateTime CreationTime;
        private string dueDate;
        private int taskId;
        private string emailAssignee;
        int boardID;
        int columnOrdinal;

        public Task(int boardID, int columnOrdinal, string title, string description, DateTime dueDate, int taskId, string emailAssignee)
        {
            //Check that the title is not empty.
            if (validator.validateTitle(title))
            {
                this.title = title;
            }
            //Check that the description is not empty.
            if (validator.validateDes(description))
            {
                this.description = description;
            }
            //Using casting on DateTime type in order to change it.
            string dt = dueDate.ToString();
            this.CreationTime = DateTime.Now;
            if (dueDate > CreationTime)
            {
                this.dueDate = dt;
            }
            else
            {
                log.Warn("DueTime already passed");
                throw new Exception("DueTime already passed");
            }
            this.emailAssignee = emailAssignee;
            this.taskId = taskId;
            this.boardID = boardID;
            this.columnOrdinal = columnOrdinal;
            DataAccessLayer.DALTaskController DTC = new DataAccessLayer.DALTaskController();
            DTC.Insert(ToDalObject());
        }
        public Task(int boardID, int columnOrdinal, string title, DateTime dueDate, int taskId, string emailAssignee) //constructor with no description
        {
            //Check that the title is not empty.
            if (validator.validateTitle(title))
            {
                this.title = title;
            }
            //Using casting on DateTime type in order to change it.
            this.description = null;
            string dt = dueDate.ToString();
            this.CreationTime = DateTime.Now;
            if (dueDate > CreationTime)
            {
                this.dueDate = dt;
            }
            else
            {
                log.Warn("DueTime already passed");
                throw new Exception("DueTime already passed");
            }
            this.emailAssignee = emailAssignee;
            this.taskId = taskId;
            this.boardID = boardID;
            this.columnOrdinal = columnOrdinal;
            DataAccessLayer.DALTaskController DTC = new DataAccessLayer.DALTaskController();
            DTC.Insert(ToDalObject());
        }
        public Task(DataAccessLayer.DTask task)//constructor for DATA objects
        {
            if (task.description.Equals(" "))//A null description is saved as " " because database doesnt handle null value
                this.description = null;
            else
            {
                this.description = task.description;
            }
            this.columnOrdinal = task.column;
            this.dueDate = task.dueDate;
            this.CreationTime = task.creationTime;
            this.title = task.title;
            this.taskId = task.ID;
            this.boardID = task.boardId;
            this.emailAssignee = task.emailAssignee;
        }
        public string getEmailAssignee() { return emailAssignee; }
        public void setEmailAssignee(string email)
        {
            this.emailAssignee = email;
            //database
            DataAccessLayer.DALTaskController DTC = new DataAccessLayer.DALTaskController();
            DTC.Update(boardID, taskId, "emailAssignee", email);
        }
        public void setColumnOrdinal(int neword)
        {
            columnOrdinal = neword;
            DataAccessLayer.DALTaskController DTC = new DataAccessLayer.DALTaskController();
            DTC.Update(boardID, taskId, "column", neword);
        }

        public int getTaskId() { return taskId; }
        public string getemailAssignee() { return emailAssignee; }
        public DateTime getCreationTime() { return CreationTime; }
        public string getTitle() { return title; }
        public string getDesctiption() { return description; }
        public DateTime getDueDate() { return DateTime.Parse(dueDate); }

        /// <summary>
        /// This method changes the discription of the task
        /// </summary>
        /// <param name="newDescription"></param>The new discription that we want to insert to the task.
        public void changeDescription(string email,string newDescription)
        {
            if (email != emailAssignee)
            {
                log.Warn("Only the task assignee can edit thiss task");
                throw new Exception("Only the task assignee can edit thiss task");
            }
            //Check that the new description is not empty or not too long.
            if (validator.validateDes(newDescription))
            {
                this.description = newDescription;
                //database
                DataAccessLayer.DALTaskController DTC = new DataAccessLayer.DALTaskController();
                DTC.Update(boardID, taskId, "description", newDescription);
            }
            else
            {
                //Send a message to the user
                log.Warn("The description is too long");
                throw new Exception("The description is too long");
            }
        }
        /// <summary>
        /// This method changes the title of the task
        /// </summary>
        /// <param name="newTitle"></param>The new title that we want to insert to the task.
        public void changeTitle(string email,string newTitle)
        {
            if (email != emailAssignee)
            {
                log.Warn("Only the task assignee can edit thiss task");
                throw new Exception("Only the task assignee can edit thiss task");
            }
            //Check that the new title is not empty or not too long.
            if (validator.validateTitle(newTitle))
            {
                this.title = newTitle;
            }
            //Send a message to the user
            else
            {
                log.Warn("Invalid title");
                throw new Exception("Invalid title");
            }
            DataAccessLayer.DALTaskController DTC = new DataAccessLayer.DALTaskController();
            DTC.Update(boardID, taskId, "title", newTitle);
        }
        /// <summary>
        /// The new duedate that we want to insert to the task.
        /// </summary>
        /// <param name="newDueDate"></param>The new duedate that we want to insert to the task.
        public void changeDueDate(string email,DateTime newDueDate)
        {
            if (email != emailAssignee)
            {
                log.Warn("Only the task assignee can edit thiss task");
                throw new Exception("Only the task assignee can edit thiss task");
            }
            this.dueDate = newDueDate.ToString();
            //database
            DataAccessLayer.DALTaskController DTC = new DataAccessLayer.DALTaskController();
            DTC.Update(boardID, taskId, "dueDate", newDueDate.ToString());
        }
        public DataAccessLayer.DTask ToDalObject()
        {
            if (description == null)//database doesnt handle null
            {
                description = " ";
            }
            DataAccessLayer.DTask toReturn = new DataAccessLayer.DTask(boardID, taskId, columnOrdinal, title, description, dueDate, CreationTime, emailAssignee);
            return toReturn;
        }
    }
}