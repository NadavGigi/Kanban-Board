using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class AssignTaskViewModel : NotifiableObject
    {
        private string emailAsignee;
        public string EmailAsignee {
            get {
                return emailAsignee;
            }
            set {
                emailAsignee = value;
                RaisePropertyChanged("EmailAsignee");
            }
        }
        private string message = "";
        public string Message {
            get {
                return message;
            }
            set {
                message = value;
                RaisePropertyChanged("Message");
            }
        }
        public TaskModel task;
        private string userEmail;
        private string boardName;
        private string creatorEmail;
        private int ordinal;
        //Constructor -------------------------------------------------------------------------------------------------------------
        public AssignTaskViewModel(TaskModel task, string userEmail, string creatorEmail, string boardName, int ordinal)
        {
            this.userEmail = userEmail;
            this.boardName = boardName;
            this.creatorEmail = creatorEmail;
            this.ordinal = ordinal;
            this.task = task;
        }
        //Methods -------------------------------------------------------------------------------------------------------------

        public bool AssignTask()
        {
            Message = "";
            try
            {
                task.bc.AssignTask(userEmail, creatorEmail, boardName, ordinal, task.TaskId, EmailAsignee);
                task.EmailAssignee = EmailAsignee;
                if (task.EmailAssignee.Equals(userEmail))
                {
                    task.BackGroundColor = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    task.BackGroundColor = new SolidColorBrush(Colors.White);
                }
            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message
                return false;
            }
            return true;
        }
    }
}