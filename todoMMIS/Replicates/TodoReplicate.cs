using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Replicates
{
    public class TodoReplicate:BaseReplicate
    {
        public ApplicationContext AppContext { get; }
        public EFTodo Model { get; set; }
        public TodoReplicate(ApplicationContext appContext, EFTodo model) : base(appContext, model)
        {
            AppContext = appContext;
            Model = model;
        }
    }
}
