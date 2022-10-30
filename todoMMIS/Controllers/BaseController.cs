using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Controllers
{
    public class BaseController: Controller
    {
        public ApplicationContext _appContext { get; }

        public BaseController(ApplicationContext appContext)
        {
            _appContext = appContext;
        }
        internal dynamic GetCommon()
        {
            dynamic common = new ExpandoObject();

            return common;
        }

        internal JsonResult Send(string message)
        {
            return Json(new Answer()
            {
                Message = message,
                Data = DateTime.Now,
            });
        }
        internal JsonResult Execute(Func<DBContext, JsonResult> action, string expMessage)
        {
            DBContext dbContext = _appContext.CreateDbContext();

            try { return action(dbContext); }
            catch { return Send(expMessage); }
        }
    }
}
