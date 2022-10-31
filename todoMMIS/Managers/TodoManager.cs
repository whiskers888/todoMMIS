using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Contexts;
using WebUI.EFModels;
using WebUI.Replicates;

namespace WebUI.Managers
{
    public class TodoManager : BaseManager<TodoReplicate, EFTodo>
    {
        public TodoManager(ApplicationContext app) : base(app)
        {

        }
    }
}
