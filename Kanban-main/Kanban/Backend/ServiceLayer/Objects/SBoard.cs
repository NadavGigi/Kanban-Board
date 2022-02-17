using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct SBoard
    {
        public readonly string name { get; }
        public readonly string emailCreator;
        public readonly List<SColumn> columns { get; }
        public readonly int boardID;
        public readonly List<string> members;
        public readonly int taskID;
        internal SBoard(Board board)
        {
            List<SColumn> columns = new List<SColumn>();
            List<Column> ColumnstoConvert = board.getAllColumns();
            foreach (Column c in ColumnstoConvert)
            {
                SColumn newSColumn = new SColumn(c);
                columns.Add(newSColumn);
            }
            this.columns = columns;
            this.emailCreator = board.GetEmailofcreator();
            this.boardID = board.getBoardID();
            this.name = board.GetBoardName();
            this.members = board.getBoardMembers();
            this.taskID = board.GetTaskId();
        }
    }
}
