using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Replicates
{
    public class TodoReplicate : BaseReplicate
    {
        internal EFTodo Context { get; set; }
        public TodoReplicate(ApplicationContext app, EFTodo _context) : base(app, _context)
        {
            Context = _context;
        }

        public string TaskDescription
        {
            get => Context.TaskDescription;
            set => Context.TaskDescription = value;
        }
        public string User
        {
            get => Context.User;
            set => Context.User = value;
        }
        public bool IsComplete
        {
            get => Context.IsComplete;
            set => Context.IsComplete = value;
        }

        public bool IsExpired
        {
            get => Context.IsExpired = DateTime.Now.ToLocalTime() >= DateExpired ? true : false;
            set => Context.IsExpired = value;
        }

        public DateTime DateCreate
        {
            get => Context.DateCreate;
            set => Context.DateCreate = value;
        }

        public DateTime DateExpired
        {
            get => Context.DateExpired;
            set => Context.DateExpired = value;
        }
        public int Priority
        {
            get => Context.Priority;
            set => Context.Priority = value;
        }

    }
}
