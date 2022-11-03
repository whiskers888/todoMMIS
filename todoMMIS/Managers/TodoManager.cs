using System;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class TodoManager:BaseManager<TodoReplicate,EFTodo>
    {
        public ApplicationContext AppContext { get; }
        public TodoReplicate Replicate { get; set; }
        public EFTodo TodoItem { get; set; }
        public TodoManager(ApplicationContext app) : base(app)
        {
            AppContext = app;

        }

        
    }
}
