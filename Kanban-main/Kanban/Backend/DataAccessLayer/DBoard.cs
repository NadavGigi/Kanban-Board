using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DBoard : DAL
    {
        public const string boardEmailOfCreator = "emailCreator";
        public const string boardTaskID = "taskId";//counter
        public const string boardBoardId = "boardId";
        public const string boardName = "name";
        public const string boardcolumnCounter = "columnID";
        public string emailOfCreator { get; set; }
        public int boardId { get; set; }
        public int taskID { get; set; }
        public string name { get; set; }
        public int columnID { get; set; }
        public DBoard(int boardId, string emailOfCreator, string name, int taskID,int columnID) : base(new DALBoardController())
        {
            this.boardId = boardId;
            this.emailOfCreator = emailOfCreator;
            this.taskID = taskID;
            this.name = name;
            this.columnID = columnID;
        }

    }
}