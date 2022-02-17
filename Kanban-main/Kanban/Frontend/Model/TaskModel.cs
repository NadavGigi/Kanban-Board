using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace Presentation.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private string title;
        private string description;
        private DateTime creationTime;
        private DateTime dueDate;
        private int taskId;
        private string emailAssignee;
        public string Title {
            get { return title; }
            set {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Description {
            get { return description; }
            set {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        public DateTime CreationTime {
            get { return creationTime; }
            set {
                creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        public DateTime DueDate {
            get { return dueDate; }
            set {
                dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        public int TaskId {
            get { return taskId; }
            set {
                taskId = value;
                RaisePropertyChanged("TaskId");
            }
        }
        public string EmailAssignee {
            get { return emailAssignee; }
            set {
                emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }
        private SolidColorBrush backGroundColor;
        public SolidColorBrush BackGroundColor {
            get { return backGroundColor; }
            set {
                this.backGroundColor = value;
                RaisePropertyChanged("BackGroundColor");
            }
        }
        private SolidColorBrush fontColor;
        public SolidColorBrush FontColor {
            get { return fontColor; }
            set {
                this.fontColor = value;
                RaisePropertyChanged("FontColor");
            }
        }

        public TaskModel(string emailAssignee, DateTime dueDate, string title, string description, int taskId, BackendController bc) : base(bc)
        {
            this.EmailAssignee = emailAssignee;
            this.CreationTime = DateTime.Now;
            this.DueDate = dueDate;
            this.Title = title;
            this.TaskId = taskId;
            this.Description = description;
            BackGroundColor = new SolidColorBrush(Colors.Blue);
        }
        public TaskModel(STask st, BackendController bc) : base(bc)
        {
            this.EmailAssignee = st.emailAssignee;
            this.CreationTime = st.CreationTime;
            this.DueDate = st.DueDate;
            this.Title = st.Title;
            this.TaskId = st.Id;
            this.Description = st.Description;
            this.Description = description;
            FontColor = findFontColor(creationTime, dueDate);
            BackGroundColor = new SolidColorBrush(Colors.Blue);

        }
        private SolidColorBrush findFontColor(DateTime creationTime, DateTime dueDate)
        {

            double diffrence = (dueDate - creationTime).TotalDays; // all the time
            double diffrence12 = (DateTime.Now - creationTime).TotalDays; // past time

            if (dueDate.CompareTo(DateTime.Now) < 0)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                if (diffrence * 0.75 < diffrence12)
                    return new SolidColorBrush(Colors.Orange);
                else
                    return new SolidColorBrush(Colors.White);
            }
        }
    }
}
