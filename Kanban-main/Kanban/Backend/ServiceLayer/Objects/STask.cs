using System;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct STask
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly string Title;
        public readonly string Description;
        public readonly DateTime DueDate;
        public readonly string emailAssignee;
        internal STask(Task task)
        {
            this.Id = task.getTaskId();
            this.CreationTime = task.getCreationTime();
            this.Title = task.getTitle();
            this.Description = task.getDesctiption();
            this.DueDate = task.getDueDate();
            this.emailAssignee = task.getemailAssignee();
        }
}
}
