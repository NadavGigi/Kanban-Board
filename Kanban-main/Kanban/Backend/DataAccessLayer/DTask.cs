using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DTask : DAL
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DTask");

        public const string taskBoardId = "boardId";
        public const string taskId = "taskId";
        public const string taskColumn = "column";
        public const string taskTitle = "title";
        public const string taskDescription = "description";
        public const string taskDueDate = "dueDate";
        public const string taskCreationTime = "creationTime";
        public const string taskEmailAssignee = "emailAssignee";
        public int column { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string dueDate { get; set; }
        public DateTime creationTime { get; set; }
        public int ID { get; set; }
        public int boardId { get; set; }
        public string emailAssignee { get; set; }

        public DTask(int boardId, int ID, int column, string title, string description, string dueDate, DateTime creationTime, string emailAssignee) : base(new DALTaskController())
        {
            this.boardId = boardId;
            this.column = column;
            this.ID = ID;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.creationTime = creationTime;
            this.emailAssignee = emailAssignee;
        }
    }
}