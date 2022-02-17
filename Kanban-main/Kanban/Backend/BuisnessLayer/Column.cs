using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Column
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.Column");
        private Dictionary<int, Task> Tasks;
        private int Limit = -1;//the limit that the client seting-if limit=-1 it means theres no limit
        private int SizeOfColumn = 0;//size tracker
        private int boardID;
        private int columnOrdinal;
        private string columnName;
        public Column(int boardID, int columnOrdianl, string columnName)
        {
            this.Tasks = new Dictionary<int, Task>();
            this.boardID = boardID;
            this.columnOrdinal = columnOrdianl;
            this.columnName = columnName;
            DataAccessLayer.DALColumnController DCC = new DataAccessLayer.DALColumnController();
            DCC.Insert(toDalObject());
        }
        public Column(DataAccessLayer.DColumn dcolumn, Dictionary<int, Task> tasks)
        {
            this.Tasks = tasks;
            this.Limit = dcolumn.lim;
            this.boardID = dcolumn.boardId;
            this.columnOrdinal = dcolumn.ordinal;
            this.SizeOfColumn = dcolumn.sizeOfColumn;
            this.columnName = dcolumn.name;
        }

        public int getColumnSize() { return SizeOfColumn; }
        /// <summary>
        /// seting a limit to the size of the dictionary
        /// </summary>
        /// <param name="lim">the max tasks in a column</param>
        public void SetLimit(int lim)
        {
            if (lim < SizeOfColumn)
            {
                log.Warn("the limit you choose is smaller then the amount of tasks");
                throw new Exception("the limit you choose is smaller then the amount of tasks");
            }
            this.Limit = lim;
            //updating data base
            DataAccessLayer.DALColumnController DCC = new DataAccessLayer.DALColumnController();
            DCC.Update(boardID, columnOrdinal, "lim", lim);
        }
        public void setColumnOrdinal(int newOrd){
            DataAccessLayer.DALColumnController DCC = new DataAccessLayer.DALColumnController();
            DCC.Update(boardID, columnOrdinal, "columnOrdinal", newOrd);
            columnOrdinal = newOrd;
            foreach(var t in Tasks.Values)
            {
                t.setColumnOrdinal(newOrd);
            }
        }

        public int getColumnOrdinal() { return columnOrdinal; }
        /// <summary>
        /// return the column lim
        /// </summary>
        /// <returns></returns>
        public int getLim() { return this.Limit; }
        /// <summary>
        /// adding a new task to the dictionay 
        /// </summary>
        /// <param name="newTask">the task that been added</param>
        public string getName()
        {
            return columnName;
        }
        public void Add(Task newTask)

        {
            if (Limit == SizeOfColumn)
                throw new Exception("youve reached your coulmn limit");
            Tasks.Add(newTask.getTaskId(), newTask);
            SizeOfColumn++;
            //updating data base
            DataAccessLayer.DALColumnController DCC = new DataAccessLayer.DALColumnController();
            DCC.Update(boardID, columnOrdinal, "size", SizeOfColumn);
        }
        /// <summary>
        /// removing a task from the dictionay and updating the size
        /// </summary>
        /// <param name="taskID"></param>
        public void RemoveTask(int taskID)
        {
            Task task = getTask(taskID);//check if the task is in the list
            Tasks.Remove(taskID);
            SizeOfColumn--;
            //updating data base
            DataAccessLayer.DALColumnController DCC = new DataAccessLayer.DALColumnController();
            DCC.Update(boardID, columnOrdinal, "size", SizeOfColumn);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Task> GetTaskList()
        //returns a list with task elemnts
        {
            return Tasks.Values.ToList<Task>();
        }
        public Task getTask(int taskID)
        //returns a task using its id
        {
            if (!(Tasks.ContainsKey(taskID)))
            {
                log.Warn("task was not found");
                throw new Exception("task was not found");
            }
            return Tasks[taskID];
        }
        public DataAccessLayer.DColumn toDalObject() //Converts BusinessUser to 
        {

            DataAccessLayer.DColumn toReturn = new DataAccessLayer.DColumn(this.boardID, this.columnOrdinal, this.Limit, this.SizeOfColumn,this.columnName);
            log.Debug("Converted column to DAL Object succesfully");
            return toReturn;
        }
        public void addTasksList (List<Task> taskslist)
        {
            if (Limit != -1)
            {
                if (taskslist.Count() + SizeOfColumn > Limit)
                {
                    throw new Exception("while trying to delete a column-the left/right column is full");
                }
            }
            foreach(var item in taskslist)
            {
                item.setColumnOrdinal(columnOrdinal);
                Add(item);
            }
            
        }

        internal void setName(string newColumnName)
        {
            this.columnName = newColumnName;
            DataAccessLayer.DALColumnController DCC = new DataAccessLayer.DALColumnController();
            DCC.Update(boardID, columnOrdinal, "name", newColumnName);
        }
    }
}