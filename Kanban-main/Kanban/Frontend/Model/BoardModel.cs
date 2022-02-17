using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private ObservableCollection<ColumnModel> columns;
        public ObservableCollection<ColumnModel> Columns {
            get {
                return columns;
            }
            set {
                columns = value;
                RaisePropertyChanged("Columns");
            }
        }
        private string boardName;
        public string BoardName {
            get {
                return boardName;
            }
            set {
                boardName = value;
                RaisePropertyChanged("BoardName");
            }
        }
        private string creatorEmail;
        public string CreatorEmail {
            get {
                return creatorEmail;
            }
            set {
                creatorEmail = value;
                RaisePropertyChanged("CreatorEmail");
            }
        }
        private int taskId;
        public int TaskId {
            get {
                return taskId;
            }
            set {
                taskId = value;
                RaisePropertyChanged("TaskId");
            }
        }


        public BoardModel(SBoard Sb,BackendController bc) : base(bc)
        {
            this.CreatorEmail = Sb.emailCreator;
            this.BoardName = Sb.name;
            this.Columns = bc.GetAllColumns(Sb);
            Columns = new ObservableCollection<ColumnModel>(Columns.OrderBy(T => T.Ordinal));
            this.TaskId = Sb.taskID;
        }

        public BoardModel(BackendController bc, string BoardName) : base(bc)
        {
            this.BoardName = BoardName;
        }
        public ColumnModel getColumn(int ordinal)
        {
            if (ordinal >= 0 & ordinal < Columns.Count)
            {
                foreach (ColumnModel item in Columns)
                {
                    if (ordinal == item.Ordinal)
                        return item;
                }

            }
            return null;
        }
        public ColumnModel getColumn(string columnName)
        {
            foreach (var item in Columns)
            {
                if (item.Name.Equals(columnName))
                {
                    return item;
                }
            }
            return null; //If column was not found
        }
    }
}
