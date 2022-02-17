using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class AddTaskViewModel : NotifiableObject
    {
        //Properties-------------------------------------------------------------------------------
        public BoardModel board;
        public BoardModel Board {
            get {
                return board;
            }
            set {
                board = value;
                RaisePropertyChanged("Board");
            }
        }
        public string userEmail;
        private DateTime dueDate = DateTime.Now;
        public DateTime DueDate {
            get {
                return dueDate;
            }
            set {
                dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        private string title;
        public string Title {
            get {
                return title;
            }
            set {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
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
        //Constructor-------------------------------------------------------------------------------
        public AddTaskViewModel(string userEmail, BoardModel Board)
        {
            this.userEmail = userEmail;
            this.Board = Board;
        }
        //Methods-------------------------------------------------------------------------------

        public bool AddTask()
        {
            try
            {
                Board.bc.AddTask(userEmail, Board.CreatorEmail, Board.BoardName, Title, Description, DueDate);
                TaskModel NewTask = new TaskModel(userEmail, DueDate, Title, Description, Board.TaskId, Board.bc);
                Board.getColumn(0).Tasks.Add(NewTask);
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
