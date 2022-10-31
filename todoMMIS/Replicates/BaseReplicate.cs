using WebUI.Contexts;
using WebUI.EFModels;

namespace WebUI.Replicates
{
    public class BaseReplicate
    {
        EFBaseModel Context { get; set; }
        protected ApplicationContext App { get; }
        public BaseReplicate(ApplicationContext _app, EFBaseModel _context)
        {
            App = _app;
            Context = _context;
        }
        public int Id => Context.Id;
    }
}
