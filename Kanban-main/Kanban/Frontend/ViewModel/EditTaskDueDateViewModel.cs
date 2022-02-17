using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class EditTaskDueDateViewModel : NotifiableObject
    {
        private DateTime dueDate;
        public DateTime DueDate {
            get {
                return dueDate;
            }
            set {
                dueDate = value;
                RaisePropertyChanged("DueDate");
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
        public EditTaskDueDateViewModel(TaskModel task, string userEmail, string creatorEmail, string boardName, int ordinal)
        {
            this.userEmail = userEmail;
            this.boardName = boardName;
            this.creatorEmail = creatorEmail;
            this.ordinal = ordinal;
            this.task = task;
        }
        //Methods -------------------------------------------------------------------------------------------------------------

        public bool UpdateDueDate()
        {
            Message = "";
            try
            {
                task.bc.UpdateTaskDueDate(userEmail, creatorEmail, boardName, ordinal, task.TaskId, DueDate);
                task.DueDate = DueDate;
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
