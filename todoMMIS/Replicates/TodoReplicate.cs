using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Contexts;
using WebUI.EFModels;

namespace WebUI.Replicates
{
    public class TodoReplicate : BaseReplicate
    {
        EFTodo Context { get; set; }
        public TodoReplicate(ApplicationContext app, EFTodo _context) : base(app, _context)
        {
            Context = _context;
        }

        public string Name => Context.Name;
    }
}
