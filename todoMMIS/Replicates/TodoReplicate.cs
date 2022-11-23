using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoMMIS.Contexts;
using todoMMIS.Models.EF;

namespace todoMMIS.Replicates
{
    public class TodoReplicate : BaseReplicate
    {
        protected EFTodo Context { get; set; }
        public TodoReplicate (ApplicationContext app, EFTodo _context) : base(app, _context)
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
        public bool IsCompleted
        {
            get => Context.IsCompleted;
            set => Context.IsCompleted = value;
        }
        public bool IsExpired
        {
            get => Context.IsExpired = DateTime.Now.ToLocalTime() >= expiredAt ? true : false;
            set => Context.IsExpired = value;
        }
        public DateTime? createdAt
        {
            get => Context.createdAt;
            set => Context.createdAt = value;
        }
        public DateTime? expiredAt
        {
            get => Context.expiredAt;
            set => Context.expiredAt = value;
        }
        public int Priority
        {
            get => Context.Priority;
            set => Context.Priority = value;
        }
    }
}
