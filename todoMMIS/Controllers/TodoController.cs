using todoMMIS.Contexts;

namespace todoMMIS.Controllers
{
    public class TodoController:BaseController
    {
        public TodoController(ApplicationContext appContext) : base(appContext)
        {

        }
    }
}
