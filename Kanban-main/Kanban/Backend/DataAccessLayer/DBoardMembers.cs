using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DBoardMembers : DAL
    {
        public const string boardMEmail = "email";
        public const string boardMBoardID = "boardId";
        public string email { get; set; }
        public int boardId { get; set; }
        public DBoardMembers(int boardId, string email) : base(new DALBoardMembersController())
        {
            this.boardId = boardId;
            this.email = email;
        }

    }
}