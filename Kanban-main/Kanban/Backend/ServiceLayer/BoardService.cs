using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BoardService");
        private BuisnessLayer.BoardController bc = new BuisnessLayer.BoardController();

        public BoardService()
        {

        }

        /// <summary>
        /// limiting columns by identified by column ordianl
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email">email of the person who conducts the change.</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit. </param>
        public Response LimitColumn(string email, string creatorEmail, string boardName, int columnOrdinal, int limit, BoardController bc, UserController uc)
        {
            try
            {
                uc.validateLogin(email);
                bc.LimitColumn(email, creatorEmail, boardName, columnOrdinal, limit);

                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// get column name specific column ordinal
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email">email of the person who conducts the change.</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is not found</exception>
        public Response<int> GetColumnLimit(string email, string creatorEmail, string boardName, int columnOrdinal, BoardController bc, UserController uc)

        {
            try
            {
                uc.validateLogin(email);
                int columnlimit = bc.GetColumnLimit(email, creatorEmail, boardName, columnOrdinal);
                return Response<int>.FromValue(columnlimit);
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }

        }

        /// <summary>
        /// get the column name
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email">email of the person who conducts the change.</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>string of the column name</returns>

        public Response<string> GetColumnName(string email, string creatorEmail, string boardName, int columnOrdinal, BoardController bc, UserController uc)

        {
            try
            {
                uc.validateLogin(email);
                string columname = bc.GetColumnName(email, creatorEmail, boardName, columnOrdinal);
                return Response<string>.FromValue(columname);
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }

        /// <summary>
        /// adding a task
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email">email of user</param>
        /// <param name="boardName">board name</param>
        /// <param name="title">title</param>
        /// <param name="description">description</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>a task that have just built</returns>
        public Response<STask> AddTask(string email, string creatorEmail, string boardName, string title, string description, DateTime dueDate, BoardController bc, UserController uc)

        {
            try
            {
                uc.validateLogin(email);
                BuisnessLayer.Task taskB = bc.AddTask(email, creatorEmail, boardName, title, description, dueDate);
                STask task = new STask(taskB);
                return Response<STask>.FromValue(task);
            }
            catch (Exception e)
            {
                return Response<STask>.FromError(e.Message);
            }
        }

        /// <summary>
        /// updating a task due date
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email">email of creator</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        public Response UpdateTaskDueDate(string email, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate, BoardController bc, UserController uc)
        {
            try
            {
                uc.validateLogin(email);
                bc.UpdateTaskDueDate(email, creatorEmail, boardName, columnOrdinal, taskId, dueDate);

                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// updating a task title
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        public Response UpdateTaskTitle(string email, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title, BoardController bc, UserController uc)

        {
            try
            {
                uc.validateLogin(email);
                bc.UpdateTaskTitle(email, creatorEmail, boardName, columnOrdinal, taskId, title);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// updating a task description
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="description"></param>

        public Response UpdateTaskDescription(string email, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description, BoardController bc, UserController uc)
        {
            try
            {
                uc.validateLogin(email);
                bc.UpdateTaskDescription(email, creatorEmail, boardName, columnOrdinal, taskId, description);

                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// advancing a task 
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        public Response AdvanceTask(string email, string creatorEmail, string boardName, int columnOrdinal, int taskId, BoardController bc, UserController uc)
        {
            try
            {
                uc.validateLogin(email);
                bc.AdvanceTask(email, creatorEmail, boardName, columnOrdinal, taskId);

                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// get a column using email board name and column ordinal as identifiers
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>

        public Response<IList<STask>> GetColumn(string email,string creatorEmail, string boardName, int columnOrdinal, BoardController bc,UserController uc)

        {
            List<STask> newlist = new List<STask>();
            try
            {
                uc.validateLogin(email);
                bc.GetColumn(email, creatorEmail, boardName, columnOrdinal);
                List<BuisnessLayer.Task> tasks = bc.GetColumn(email, creatorEmail, boardName, columnOrdinal);
                foreach (var t in tasks)
                {
                    STask task = new STask(t);
                    newlist.Add(task);
                }
                return Response<IList<STask>>.FromValue(newlist);
            }
            catch (Exception e)
            {
                return Response<IList<STask>>.FromError(e.Message);
            }
        }

        /// <summary>
        /// adding a Board
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">name of board</param>
        /// <exception cref="System.Exception">throw an exeption if the board is already exists</exception>

        public Response AddBoard(string email, string name, BoardController bc, UserController uc)

        {
            try
            {
                uc.validateLogin(email);
                bc.AddBoard(email, name);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// removing a board from the dictionary
        /// </summary>
        /// <param name="email">email of creator</param>
        /// <param name="name">name of the board</param>
        /// <exception cref="System.Exception">throw an exeption if the board is not exists</exception>
        public Response RemoveBoard(string email, string creatorEmail, string name, BoardController bc, UserController uc)

        {
            try
            {
                uc.validateLogin(email);
                bc.removeBoard(email, creatorEmail, name);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// check if the user is logged in the 
        /// </summary>
        /// <param name="email">email of user</param>
        /// <returns> a IList with in progress tasks</returns>
        public Response<IList<STask>> InProgressTasks(string email, BoardController bc)
        {
            try
            {
                IList<STask> taskList = new List<STask>();
                IList<BuisnessLayer.Task> lt = bc.inProgressTasks(email);
                foreach (var t in lt)
                {
                    taskList.Add(new STask(t));
                }
                log.Info("Extraced Progress column successfully");
                return Response<IList<STask>>.FromValue(taskList);
            }
            catch (Exception e)
            {
                return Response<IList<STask>>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Join to existing board using your email the board name and board's creator email.
        /// </summary>
        /// <param name="email">email of the user who join the board.</param>
        /// <param name="boardName">board name</param>
        /// <param name="creatorEmail">Board's email creator
        public Response JoinBoard(string userEmail, string creatorEmail, string boardName, UserController uc)

        {
            try
            {
                uc.validateLogin(userEmail);
                bc.getBoard(creatorEmail, boardName).JoinBoard(userEmail);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// assing a task to a specific person.
        /// </summary>
        /// <param name="creatorEmail">email of creator</param>
        /// <param name="taskId">id of the task</param>
        /// <param name="email">email of the person who conducts the change.</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">throw an exeption if the column ordinal is not found</exception>

        public Response AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee, UserController uc)

        {
            try
            {
                uc.validateLogin(userEmail);
                bc.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// return a list contains the board memebers.
        /// </summary>
        /// <param name="userEmail">email of the user</param>
        /// <returns></returns>
        public Response<IList<string>> GetBoardNames(string userEmail, UserController uc)

        {
            try
            {
                uc.validateLogin(userEmail);
                IList<string> boardNames = new List<string>();
                bc.GetBoardsName(userEmail);
                boardNames = bc.GetBoardsName(userEmail);
                return Response<IList<string>>.FromValue(boardNames);
            }
            catch (Exception e)
            {
                return Response<IList<string>>.FromError(e.Message);
            }
        }
        ///<summary>This method loads the data from the persistance.
        ///         You should call this function when the program starts. </summary>
        public Response loadData(BoardController bc)
        {
            try
            {
                bc.LoadData();
                return new Response();
            }
            catch (Exception e)
            {
                log.Warn("load Data failed");
                return new Response(e.Message);
            }
        }

        internal Response AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName,UserController uc)
        {
            try
            {
                uc.validateLogin(userEmail);
                bc.AddColumn(userEmail, creatorEmail, boardName, columnOrdinal, columnName);
                return new Response();
            }
            catch (Exception e)
            {
                log.Warn("Adding Column faild");
                return new Response(e.Message);
            }
        }

        internal Response RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, UserController uc)
        {
            try
            {
                uc.validateLogin(userEmail);
                bc.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
                return new Response();
            }
            catch (Exception e)
            {
                log.Warn("removing Column faild");
                return new Response(e.Message);
            }
        }
        public Response<SBoard> getSBoard(string userEmail, string boardName,UserController uc)
        {
            try
            {
                Board b = bc.getBoard(userEmail,boardName);
                SBoard NewBoard = new SBoard(b);
                return Response<SBoard>.FromValue(NewBoard);
            }
            catch(Exception e)
            {
                log.Warn("No Board was found");
                return Response<SBoard>.FromError(e.Message);
            }
            
        }

        internal Response RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName, UserController uc)
        {
            try
            {
                uc.validateLogin(userEmail);
                bc.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, newColumnName);
                return new Response();
            }
            catch (Exception e)
            {
                log.Warn("Renaming Column faild");
                return new Response(e.Message);
            }
        }

        internal Response MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize, UserController uc)
        {
            try
            {
                uc.validateLogin(userEmail);
                bc.MoveColumn(userEmail,creatorEmail, boardName, columnOrdinal, shiftSize);
                return new Response();
            }
            catch (Exception e)
            {
                log.Warn("Moving Column faild");
                return new Response(e.Message);
            }
        }
        internal Response<List<SBoard>> GetAllBoards(string userEmail,UserController uc)
        {
            try
            {
                uc.validateLogin(userEmail);
                List<SBoard> toReturn = new List<SBoard>();
                List<Board> allUserBoards = bc.GetAllBoards(userEmail);
                foreach (Board b in allUserBoards)
                {
                    SBoard toAdd = new SBoard(b);
                    toReturn.Add(toAdd);
                }
                return Response<List<SBoard>>.FromValue(toReturn);
            }
            catch(Exception e)
            {
                return Response<List<SBoard>>.FromError(e.Message);
            }
        }
        internal Response<List<SBoard>> GetBoards(string userEmail, UserController uc)
        {
            try
            {
                uc.validateLogin(userEmail);
                List<SBoard> toReturn = new List<SBoard>();
                IList<Board> allUserBoards = bc.GetBoards(userEmail);
                foreach (Board b in allUserBoards)
                {
                    SBoard toAdd = new SBoard(b);
                    toReturn.Add(toAdd);
                }
                return Response<List<SBoard>>.FromValue(toReturn);
            }
            catch (Exception e)
            {
                return Response<List<SBoard>>.FromError(e.Message);
            }
        }
    }
}