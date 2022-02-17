using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;



namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        public string email;
        public UserModel um;
        private ObservableCollection<TaskModel> tasks;
        public ObservableCollection<TaskModel> Tasks {
            get {
                return tasks;
            }
            set {
                tasks = value;
                RaisePropertyChanged("Tasks");
            }
        }
        private string name;
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        private int ordinal;
        public int Ordinal {
            get {
                return ordinal;
            }
            set {
                ordinal = value;
                RaisePropertyChanged("Ordinal");
            }
        }
        private int limit;
        public int Limit {
            get {
                return limit;
            }
            set {
                limit = value;
                RaisePropertyChanged("Limit"); 
            }
        }
        public ColumnModel(SColumn sc, BackendController bc) : base(bc)
        {
            this.Limit = sc.Limit;
            this.Name = sc.Name;
            this.Ordinal = sc.ordinal;
            this.Tasks = bc.getAllTasks(sc);
        }
        public ColumnModel(string ColumnName,int columnOrdinal, BackendController bc) : base(bc)
        {
            this.Limit = -1;
            this.Name = ColumnName;
            this.Ordinal = columnOrdinal;
        }
        public TaskModel getTask(int taskId) //if taskId doesnt exists returns null
        {
            foreach (var item in Tasks)
            {
                if (item.TaskId == taskId)
                    return item;
            }
            return null;
        }

    }

}
