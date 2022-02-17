using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;
using Presentation.View;

namespace Presentation.ViewModel
{
    public class EditTaskViewModel : NotifiableObject
    {
        public string userEmail { get; }
        public string boardName { get; }
        public string creatorEmail { get; }
        public int ordinal { get; }
        public BoardModel board { get; private set; }

        public TaskModel task { get; private set; }

        private string title;
        public string Title {
            get { return title; }
            set {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string description;
        public string Description {
            get { return description; }
            set {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        private DateTime dueDate;
        public DateTime DueDate {
            get { return dueDate; }
            set {
                dueDate = value;
                RaisePropertyChanged("DueDate");
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
        public EditTaskViewModel(TaskModel task,BoardModel board, string userEmail, string creatorEmail, string boardName, int ordinal)
        {
            this.userEmail = userEmail;
            this.creatorEmail = creatorEmail;
            this.boardName = boardName;
            this.ordinal = ordinal;
            this.task = task;
            this.board = board;
            DueDate = task.DueDate;
            Title = task.Title;
            Description = task.Description;
        }

        public AssignTaskViewModel AssignTask()
        {
            AssignTaskViewModel atvm = new AssignTaskViewModel(task, userEmail, creatorEmail, boardName, ordinal);
            return atvm;

        }
        public bool AdvanceTask()
        {
            Message = "";
            try
            {
                board.bc.AdvanceTask(userEmail, creatorEmail, boardName, ordinal, task.TaskId);
                board.getColumn(ordinal + 1).Tasks.Add(task);
                board.getColumn(ordinal).Tasks.Remove(task);
                return true;

            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }


        }
        public bool UpdateTitle()
        {
            Message = "";
            try
            {
                task.bc.UpdateTaskTitle(userEmail, creatorEmail, boardName, ordinal, task.TaskId, Title);
                task.Title = Title;
            }
            catch (Exception e)
            {
                Message = e.Message; //Raise error message
                return false;
            }
            return true;
        }
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

