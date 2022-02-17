using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using System.Collections.ObjectModel;

namespace Presentation
{
    public class BackendController
    {
        private Service service { get; set; }
        public BackendController(Service service)
        {
            this.service = service;
        }
        public BackendController()
        {
            this.service = new Service();
            service.LoadData();
        }
        public UserModel Login(string username, string password)
        {
            Response<SUser> response = service.Login(username, password);
            if (response.ErrorOccured) //if login failed
                throw new Exception(response.ErrorMessage);
            else
                return new UserModel(this, username);
        }
        public void Logout(string userEmail)
        {
            Response response = service.Logout(userEmail);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void Register(string username, string password)
        {
            Response response = service.Register(username, password);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void RemoveBoard(string username, string creatorEmail, string boardName)
        {
            Response response = service.RemoveBoard(username, creatorEmail, boardName);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public SBoard GetBoard(string username, string boardName)
        {
            Response<SBoard> response = service.getSBoard(username, boardName);

            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
                return response.Value;
        }

        public void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            Response response = service.JoinBoard(userEmail, creatorEmail, boardName);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public ObservableCollection<BoardModel> GetAllBoards(string userEmail)
        {
            Response<List<SBoard>> response = service.GetBoards(userEmail);
            ObservableCollection<BoardModel> allUserBoards = new ObservableCollection<BoardModel>();
            if (response.Value != null)
            {
                foreach (SBoard sb in response.Value)
                {
                    BoardModel NewBoard = new BoardModel(sb, this);
                    allUserBoards.Add(NewBoard);
                }
            }
            if (response.ErrorOccured)
                return allUserBoards;
            else

                return allUserBoards;
        }
        public ObservableCollection<ColumnModel> GetAllColumns(SBoard serviceBoard)
        {
            ObservableCollection<ColumnModel> allColumn = new ObservableCollection<ColumnModel>();
            foreach (SColumn sc in serviceBoard.columns)
            {
                ColumnModel newColumn = new ColumnModel(sc, this);
                allColumn.Add(newColumn);
            }
            return allColumn;
        }
        public ObservableCollection<TaskModel> getAllTasks(SColumn serviceColumn)
        {
            ObservableCollection<TaskModel> allTasks = new ObservableCollection<TaskModel>();
            foreach (STask st in serviceColumn.Tasks)
            {
                TaskModel newTask = new TaskModel(st, this);
                allTasks.Add(newTask);
            }
            return allTasks;
        }
        public IList<string> GetBoardsName(string userEmail)
        {
            Response<IList<string>> response = service.GetBoardNames(userEmail);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
                return response.Value;
        }
        public void AddBoard(UserModel user, string boardName)
        {
            Response response = service.AddBoard(user.Email, boardName);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            Response response = service.AddColumn(userEmail, creatorEmail, boardName, columnOrdinal, columnName);

            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            Response response = service.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            Response response = service.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void moveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            Response response = service.MoveColumn(userEmail, creatorEmail, boardName, columnOrdinal, shiftSize);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void moveColumnRight(string email, int columnOrdinal, ColumnModel columnToMove, ColumnModel columnFriend, BoardModel board)
        {
            Response response = service.MoveColumn(email, board.CreatorEmail ,board.BoardName, columnOrdinal, 1);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
            {
                columnToMove.Ordinal++;
                columnFriend.Ordinal--;
                board.Columns.Remove(columnToMove);
                board.Columns.Remove(columnFriend);
                board.Columns.Insert(columnFriend.Ordinal, columnFriend);
                board.Columns.Insert(columnToMove.Ordinal, columnToMove);
            }
        }
        public void moveColumnLeft(string email, int columnOrdinal, ColumnModel columnToMove, ColumnModel columnFriend, BoardModel board)
        {
            Response response = service.MoveColumn(email, board.CreatorEmail, board.BoardName, columnOrdinal, -1);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
            else
            {
                columnToMove.Ordinal--;
                columnFriend.Ordinal++;
                board.Columns.Remove(columnToMove);
                board.Columns.Remove(columnFriend);
                board.Columns.Insert(columnToMove.Ordinal, columnToMove);
                board.Columns.Insert(columnFriend.Ordinal, columnFriend);
            }
        }
            public void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string ColumnName)
        {
            Response response = service.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, ColumnName);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response response = service.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            Response response = service.LimitColumn(userEmail, creatorEmail, boardName, columnOrdinal, limit);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            Response response = service.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal,int taskId, string description)
        {
            Response response = service.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime duedate)
        {
            Response response = service.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, duedate);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
        public void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            Response response = service.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
            if (response.ErrorOccured)
                throw new Exception(response.ErrorMessage);
        }
    }
}