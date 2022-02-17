using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DColumn : DAL
    {
        public const string columnBoardId = "boardId";
        public const string columnOrdinal = "columnOrdinal";
        public const string columnLimit = "lim";
        public const string columnSize = "size";
        public const string columnName = "name";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DColumn");
        public int ordinal { get; set; }
        public int lim { get; set; }
        public int boardId { get; set; }
        public int sizeOfColumn { get; set; }
        public string name { get; set; }

        public DColumn(int boardId, int ColumnOrdinal, int lim, int sizeOfColumn,string name) : base(new DALColumnController())
        {
            this.boardId = boardId;
            this.ordinal = ColumnOrdinal;
            this.lim = lim;
            this.sizeOfColumn = sizeOfColumn;
            this.name = name;
        }
    }
}