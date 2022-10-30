using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Replicates
{
    public class TodoReplicate:BaseReplicate
    {
        public ApplicationContext AppContext { get; }
        public EFTodoItem Model { get; set; }
        public TodoReplicate(ApplicationContext appContext, EFTodoItem model) : base(appContext, model)
        {
            AppContext = appContext;
            Model = model;
        }
    }
}
