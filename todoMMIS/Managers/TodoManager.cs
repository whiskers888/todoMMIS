using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class TodoManager : BaseManager<TodoReplicate, EFTodo>
    {
        public TodoManager(ApplicationContext app) : base(app)
        {

        }
    }
}
