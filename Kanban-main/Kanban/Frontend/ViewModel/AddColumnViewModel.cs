using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class AddColumnViewModel : NotifiableObject
    {
        public BackendController bc;
        public string userEmail;
        public BoardModel board;
        private string columnName;
        public string ColumnName {
            get {
                return columnName;
            }
            set {
                columnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }
        private string message;
        public string Message {
            get {
                return message;
            }
            set {
                message = value;
                RaisePropertyChanged("Message");
            }
        }
        public AddColumnViewModel(string userEmail, BoardModel board)
        {
            this.userEmail = userEmail;
            this.board = board;
        }
        public bool AddColumn()
        {
            try
            {
                board.bc.AddColumn(userEmail, board.CreatorEmail, board.BoardName, board.Columns.Count(), ColumnName);
                ColumnModel newCoulmn = new ColumnModel(ColumnName, board.Columns.Count(), bc);
                board.Columns.Add(newCoulmn);
                return true;

            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }


        }
    }
}