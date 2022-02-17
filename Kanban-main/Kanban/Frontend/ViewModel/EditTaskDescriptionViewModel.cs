using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class EditTaskDescriptionViewModel : NotifiableObject
    {
        private string description;
        public string Description {
            get {
                return description;
            }
            set {
                description = value;
                RaisePropertyChanged("Description");
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
        public EditTaskDescriptionViewModel(TaskModel task, string userEmail, string creatorEmail, string boardName, int ordinal)
        {
            this.userEmail = userEmail;
            this.boardName = boardName;
            this.creatorEmail = creatorEmail;
            this.ordinal = ordinal;
            this.task = task;
        }
        //Methods -------------------------------------------------------------------------------------------------------------

        public bool UpdateDescription()
        {
            Message = "";
            try
            {
                task.bc.UpdateTaskDescription(userEmail, creatorEmail, boardName, ordinal, task.TaskId, Description);
                task.Description = Description;
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
