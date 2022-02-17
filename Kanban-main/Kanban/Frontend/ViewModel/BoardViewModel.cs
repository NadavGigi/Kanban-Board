using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Presentation.Model;
using Presentation.View;

namespace Presentation.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {
        public string userEmail { get; private set; }
        private bool InProgress = false;
        private BoardModel board;
        public BoardModel BackupBoard;
        public BoardModel Board {
            get {
                return board;
            }
            set {
                board = value;
                RaisePropertyChanged("Board");
            }
        }
        
        private string filterContent;
        public string FilterContent {
            get {
                return filterContent;
            }
            set {
                filterContent = value;
                RaisePropertyChanged("FilterContent");
            }
        }

        public string Title { get; private set; }

        private ColumnModel selectedColumn;
        public ColumnModel SelectedColumn {
            get {
                return selectedColumn;
            }
            set {
                selectedColumn = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedColumn");
            }
        }
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
        private bool _enableForward = false;
        public bool EnableForward {
            get => _enableForward;
            private set {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
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

        public BoardViewModel(string userEmail, BoardModel Board)
        {
            this.userEmail = userEmail;
            this.Board = Board;
            Board.Columns = new ObservableCollection<ColumnModel>(Board.Columns.OrderBy(T => T.Ordinal));
            Title = "Board Name: " + Board.BoardName;
            SetTasksBorderColors();
        }
        private TaskModel selectedTask;
        public TaskModel SelectedTask {
            get {
                return selectedTask;
            }
            set {
                selectedTask = value;
                RaisePropertyChanged("SelectedTask");
            }
        }

        public bool RemoveColumn()
        {
            Message = "";
            try
            {
                if (SelectedColumn != null)
                Board.bc.RemoveColumn(userEmail, Board.CreatorEmail, Board.BoardName, SelectedColumn.Ordinal);
                for (int i = SelectedColumn.Ordinal + 1; i < Board.Columns.Count; i++)
                {
                    Board.getColumn(i).Ordinal--;
                }

                Board.Columns.Remove(SelectedColumn);
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }
        public bool MoveRight()
        {
            Message = "";
            if (selectedColumn != null)
            {
                try
                {
                    Board.bc.moveColumnRight(userEmail, SelectedColumn.Ordinal, SelectedColumn, Board.getColumn(SelectedColumn.Ordinal + 1), Board);

                    return true;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return false;
                }
            }
            return false;
        }
        public bool MoveLeft()
        {
            Message = "";
            if (selectedColumn != null)
            {
                try
                {
                    Board.bc.moveColumnLeft(userEmail, SelectedColumn.Ordinal, SelectedColumn, Board.getColumn(SelectedColumn.Ordinal - 1), Board);
                    return true;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return false;
                }
            }
            return false;
        }
        public EditTaskViewModel TaskView()
        {
            Message = "";
 
            if (SelectedTask != null & SelectedColumn != null)
            {
                if (SelectedColumn.Tasks.Contains(SelectedTask))
                {
                    EditTaskViewModel etvm = new EditTaskViewModel(SelectedTask, Board, userEmail, Board.CreatorEmail, Board.BoardName, SelectedColumn.Ordinal);
                    return etvm;
                }
            }
            Message = "Please mark the desired Task and Column";
            return null;
        }
        public AddColumnViewModel AddColumnView()
        {
            AddColumnViewModel acvm = new AddColumnViewModel(userEmail, Board);
            return acvm;
        }
        public AddTaskViewModel AddTaskView()
        {
            AddTaskViewModel atvm = new AddTaskViewModel(userEmail,Board);
            return atvm;
        }

        public bool SetFilter()
        {
            Message = "";
            BoardModel FilteredBoard = Board;
            if (!string.IsNullOrWhiteSpace(FilterContent)) //Filtering key cant be null or white spaces
            {
                //Removing all the tasks that doesnt contain Key in their title or description from the board
                foreach (ColumnModel column in FilteredBoard.Columns)
                {
                    if (column != null)
                    {
                        foreach (TaskModel task in column.Tasks.ToList())
                        {
                            if (task.Description == null)
                            {
                                if (!task.Title.Contains(FilterContent))
                                {
                                    FilteredBoard.getColumn(column.Ordinal).Tasks.Remove(task);
                                }
                            }
                            else
                            {
                                if (!(task.Description.Contains(FilterContent) && task.Title.Contains(FilterContent)))
                                {
                                    FilteredBoard.getColumn(column.Ordinal).Tasks.Remove(task);

                                }
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }


                }
                Board = FilteredBoard;
                return true;
            }
            //Key is null or whitespaces
            Message = "please enter text to filter"; //Raise error for input 
            return false; //Failed to filter
        }
        public void SetIInProgress()
        {
            Message = "";
            if (InProgress)
            {
                Message = "You are Already watching InProgress Tasks";
            }
            else
            {
                BoardModel InProgressBoard = Board;
                if (InProgressBoard.getColumn(InProgressBoard.Columns.Count - 1).Tasks != null)
                {
                    InProgressBoard.getColumn(InProgressBoard.Columns.Count - 1).Tasks.Clear();
                }
                if (InProgressBoard.getColumn(0).Tasks != null)
                {
                    InProgressBoard.getColumn(0).Tasks.Clear();
                }
                Board = InProgressBoard;
                InProgress = true;
            }
        }
        public void ClearFilter()
        {
            Message = "";
            Board = new BoardModel(Board.bc.GetBoard(Board.CreatorEmail,Board.BoardName),Board.bc);
            SetTasksBorderColors();
            InProgress = false;
        }
        public void DueDateSort()
        {
            Message = "";
            foreach (ColumnModel column in Board.Columns)
                if(column.Tasks!= null)
                {
                    column.Tasks = new ObservableCollection<TaskModel>(column.Tasks.OrderBy(T => T.DueDate));
                }
            {
            }
        }
        public bool RenameColumn()
        {
            Message = "";
            try
            {
                Board.bc.RenameColumn(userEmail, Board.CreatorEmail,Board.BoardName , SelectedColumn.Ordinal,ColumnName);
                SelectedColumn.Name = ColumnName;
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }
        public bool SetLimit()
        {
            Message = "";
            if (selectedColumn != null)
            {
                try
                {

                    Board.bc.LimitColumn(userEmail, Board.CreatorEmail, Board.BoardName, SelectedColumn.Ordinal, Limit);
                    SelectedColumn.Limit = Limit;
                    return true;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return false;
                }
            }
            return false;
        }
        public void SetTasksBorderColors() //Colors the task's border
        {
            foreach (ColumnModel column in Board.Columns)
            {
                if (column != null)
                {
                    foreach (TaskModel task in column.Tasks)
                    {
                        if (task.EmailAssignee.Equals(userEmail))
                            task.BackGroundColor = new SolidColorBrush(Colors.Blue);
                        else
                            task.BackGroundColor = new SolidColorBrush(Colors.White);
                    }
                }

            }
        }

    }
}
