using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct SColumn
    {
        public readonly IReadOnlyCollection<STask> Tasks;
        public readonly string Name;
        public readonly int Limit;
        public readonly int ordinal;
        internal SColumn(Column column)
        {
            List<STask> Tasks = new List<STask>();
            List<BuisnessLayer.Task> TaskstoConvert = column.GetTaskList();
            foreach (BuisnessLayer.Task t in TaskstoConvert)
            {
                STask newStask = new STask(t);
                Tasks.Add(newStask);
            }
            this.Tasks = Tasks;
            this.Name = column.getName();
            this.Limit = column.getLim();
            this.ordinal = column.getColumnOrdinal();
        }
    
    }
}

