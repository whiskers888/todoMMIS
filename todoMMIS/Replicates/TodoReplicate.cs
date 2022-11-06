using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Replicates
{
    public class TodoReplicate : BaseReplicate
    {
        protected EFTodo Context { get; set; }
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
        
    }
}
