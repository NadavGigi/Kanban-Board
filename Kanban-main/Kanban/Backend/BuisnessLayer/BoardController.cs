using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using log4net;
namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class BoardController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.BoardController");
        private int boardID;//counter for  boards
        static private Dictionary<string, List<Board>> allBoards = new Dictionary<string, List<Board>>();
        public BoardController() { }
        /// <summary>
        /// adding a Board
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">name of board</param>
        /// <exception cref="System.Exception">throw an exeption if the board is already exists</exception>
        public void AddBoard(string email, string name)
        {
            bool exist = false;
            if (allBoards.ContainsKey(email))
            {
                //check if the name is already exist in the User Boards
                foreach (var b in allBoards[email])
                {
                    string bn = b.GetBoardName();
                    if (name == bn)
                    {
                        exist = true;
                        throw new Exception("Board name alredy exist");
                    }
                }
                if (!exist)
                {
                    Board newBoard = new Board(email, name, boardID);
                    //adding the Board if it not in the List
                    allBoards[email].Add(newBoard);
                }
            }
            else
            {
                Board newBoard = new Board(email, name, boardID);
                //creting a new list of board for the user and  to the list adding the Board
                List<Board> boardList = new List<Board>();
                boardList.Add(newBoard);
                allBoards.Add(email, boardList);
            }
            boardID++;
        }
        /// <summary>
        /// check if the user is logged in the 
        /// </summary>
        /// <param name="email">email of user</param>
        /// <returns> a IList with in progress tasks</returns>
        public IList<Task> inProgressTasks(string email)
        {
            IList<Task> inprogress = new List<Task>();
            IList<string> bn = GetBoardsName(email);
            //collect all thr inProgress lists from ll of the user boards
            foreach (var boards in allBoards.Values)
            {
                foreach (var board in boards)
                {
                    if (bn.Contains(board.GetBoardName()))
                    {
                        List<Task> tasks = board.getAssignTasks(email);
                        foreach (var task in tasks)
                        {
                            inprogress.Add(task);
                        }
                    }
                    else
                        continue;
                }
            }
            return inprogress;
        }
        /// <summary>
        /// get a board using email of creator and name of the board
        /// </summary>
        /// <param name="email">email of craetor</param>
        /// <param name="name">name of the board</param>
        /// <returns>a board that contain same name+email key</returns>
        /// <exception cref="System.Exception">throw an exeption if the board is not exists</exception>
        public Board getBoard(string email, string name)
        {
            //check if the email created a board
            if (allBoards.ContainsKey(email))
            {
                //check if the name exist and return it
                foreach (var b in allBoards[email])
                {
                    string bn = b.GetBoardName().ToString();
                    if (name == bn)
                    {
                        return b;
                    }
                }
            }
            throw new Exception("Board does not exist");
        }
        /// <summary>
        /// removing a board from the dictionary
        /// </summary>
        /// <param name="email">email of creator</param>
        /// <param name="name">name of the board</param>
        /// <exception cref="System.Exception">throw an exeption if the board is not exists</exception>
        public void removeBoard(string email, string creatorEmail, string name)
        {
            bool deleted = false;
            if (email != creatorEmail)
            {
                log.Warn("Only the Board creator can delete this board");
                throw new Exception("Only the Board creator can delete this board");
            }
            //check if the email created a board
            DataAccessLayer.DALBoardController DBC = new DataAccessLayer.DALBoardController();
            if (DBC.Delete(getBoard(creatorEmail, name).getBoardID()))
            {
                allBoards[creatorEmail].Remove(getBoard(creatorEmail, name));
                deleted = true;
                if (allBoards[creatorEmail].Count() == 0)
                {
                    allBoards.Remove(creatorEmail);
                }
            }    
            if (deleted)
                log.Debug("Board deleted successfuly");
            else
            {
                log.Warn("Board does not exist");
                throw new Exception("Board does not exist");
            }
        }
        /// <summary>
        /// limiting columns by identified by column ordianl
        /// </summary>
        /// <param name="email">email of creator</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit. </param>
        public void LimitColumn(string email, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            getBoard(creatorEmail, boardName).LimitColumn(email, columnOrdinal, limit);
        }
        /// <summary>
        /// get coulumn limit
        /// </summary>
        /// <param name="email">email of creator</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> the limit of the column</returns>
        public int GetColumnLimit(string email, string creatorEmail, string boardName, int columnOrdinal)
        {
            return getBoard(creatorEmail, boardName).getColumnLimit(columnOrdinal);
        }
        /// <summary>
        /// get the column name
        /// </summary>
        /// <param name="email">email of creator</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>string of the column name</returns>
        public string GetColumnName(string email, string creatorEmail, string boardName, int columnOrdinal)
        {
            return getBoard(creatorEmail, boardName).getColumnName(columnOrdinal);
        }
        /// <summary>
        /// adding a task
        /// </summary>
        /// <param name="email">email of user</param>
        /// <param name="boardName">board name</param>
        /// <param name="title">title</param>
        /// <param name="description">description</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>a task that have just built</returns>
        public Task AddTask(string email, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            return getBoard(creatorEmail, boardName).AddTask(title, description, dueDate, email);
        }
        /// <summary>
        /// updating a task due date
        /// </summary>
        /// <param name="email">email of creator</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        public void UpdateTaskDueDate(string email, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            getBoard(creatorEmail, boardName).UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
        }
        /// <summary>
        /// updating a task description
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        public void UpdateTaskDescription(string email, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            getBoard(creatorEmail, boardName).UpdateTaskDescription(email, columnOrdinal, taskId, description);
        }
        /// <summary>
        /// updating a task title
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        public void UpdateTaskTitle(string email, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            getBoard(creatorEmail, boardName).UpdateTaskTitle(email, columnOrdinal, taskId, title);
        }
        /// <summary>
        /// advancing a task 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        public void AdvanceTask(string email, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            getBoard(creatorEmail, boardName).AdvanceTask(email, columnOrdinal, taskId);
        }
        /// <summary>
        /// get a column using email board name and column ordinal as identifiers
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        public List<Task> GetColumn(string email, string creatorEmail, string boardName, int columnOrdinal)
        {
            return getBoard(creatorEmail, boardName).GetColumn(columnOrdinal).GetTaskList();
        }

        public void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            getBoard(creatorEmail, boardName).JoinBoard(userEmail);
        }

        public void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            getBoard(creatorEmail, boardName).AssignTask(userEmail, columnOrdinal, taskId, emailAssignee);
        }

        public IList<string> GetBoardsName(string userEmail)
        {
            IList<string> boardNames = new List<string>();
            userEmail = userEmail.ToLower();
            foreach (var bl in allBoards)
            {
                foreach (var b in bl.Value)
                {
                    if (b.getBoardMembers().Contains(userEmail))
                        boardNames.Add(b.GetBoardName());
                }
            }
            return boardNames;
        }
        public IList<Board> GetBoards(string userEmail)
        {
            IList<Board> boards = new List<Board>();
            userEmail = userEmail.ToLower();
            foreach (var bl in allBoards)
            {
                foreach (var b in bl.Value)
                {
                    if (b.getBoardMembers().Contains(userEmail))
                        boards.Add(b);
                }
            }
            return boards;
        }

        public void LoadData()
        {
            DataAccessLayer.DALBoardMembersController BMC = new DataAccessLayer.DALBoardMembersController();
            DataAccessLayer.DALBoardController BC = new DataAccessLayer.DALBoardController();
            DataAccessLayer.DALTaskController TC = new DataAccessLayer.DALTaskController();
            DataAccessLayer.DALColumnController CC = new DataAccessLayer.DALColumnController();
            List<DataAccessLayer.DBoardMembers> boardmem = BMC.ListOfBoardMembers();
            List<DataAccessLayer.DBoard> boards = BC.ListOfBoards();
            List<DataAccessLayer.DTask> tasks = TC.ListOfTasks();
            List<DataAccessLayer.DColumn> columns = CC.ListOfColumns();
            allBoards = new Dictionary<string, List<Board>>();
            boardID = 1;
            int max = 1;
            foreach (var Dboard in boards)
            {
                if (Dboard.boardId >= max)
                    max = Dboard.boardId + 1;
                Board BLBoard = null;//BL objects are buisnes layer objects
                Dictionary<int, Column> BLColumns = new Dictionary<int, Column>();
                foreach (var Dcolumn in columns)
                {
                    if (Dcolumn.boardId == Dboard.boardId)
                    {
                        Dictionary<int, Task> BLTasks = new Dictionary<int, Task>();
                        foreach (var Dtask in tasks)
                        {
                            if (Dtask.column == Dcolumn.ordinal & Dcolumn.boardId == Dtask.boardId) //Only add the tasks that match the correct column
                            {
                                BLTasks.Add(Dtask.ID, new Task(Dtask));//adding to task list business task which is the copy of the data task
                            }
                        }
                        Column BLColumn = new Column(Dcolumn, BLTasks);
                        BLColumns.Add(BLColumn.getColumnOrdinal(), BLColumn);
                    }
                }
                BLBoard = new Board(Dboard, BMC.Members(Dboard.boardId), BLColumns);
                if (allBoards.ContainsKey(BLBoard.GetEmailofcreator()))
                    allBoards[BLBoard.GetEmailofcreator()].Add(BLBoard);
                else
                {
                    allBoards.Add(BLBoard.GetEmailofcreator(), new List<Board>());
                    allBoards[BLBoard.GetEmailofcreator()].Add(BLBoard);
                }
            }
            boardID = max;
        }
        public void DeleteData()
        {
            DataAccessLayer.DALBoardMembersController BMC = new DataAccessLayer.DALBoardMembersController();
            DataAccessLayer.DALBoardController BC = new DataAccessLayer.DALBoardController();
            DataAccessLayer.DALTaskController TC = new DataAccessLayer.DALTaskController();
            DataAccessLayer.DALColumnController CC = new DataAccessLayer.DALColumnController();
            BMC.DeleteAll();
            BC.DeleteAll();
            TC.DeleteAll();
            CC.DeleteAll();
            allBoards = new Dictionary<string, List<Board>>();
        }

        public void AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            getBoard(creatorEmail, boardName).AddColumn(columnOrdinal, columnName,userEmail);
        }

        public void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            getBoard(creatorEmail, boardName).RemoveColumn(columnOrdinal,userEmail);
        }

        internal void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            getBoard(creatorEmail, boardName).RenameColumn(userEmail,columnOrdinal, newColumnName);
        }

        internal void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            getBoard(creatorEmail, boardName).MoveColumn(userEmail,columnOrdinal, shiftSize);
        }
        internal List<Board> GetAllBoards(string userEmail)
        {
            //check if the email created a board
            if (allBoards.ContainsKey(userEmail))
            {
                return allBoards[userEmail];
                //check if the name exist and return it
            }
            throw new Exception("User does not have any boards");
        }
    }
}