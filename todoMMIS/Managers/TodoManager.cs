using System;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class TodoManager:BaseManager
    {
        public ApplicationContext AppContext { get; }
        public TodoReplicate TodoReplicate { get; set; }
        public EFTodoItem TodoItem { get; set; }
        public TodoManager(ApplicationContext app) : base(app, replicate: TodoReplicate, model: EFTodoItem)
        {
            AppContext = app;

        }

        
    }
}
