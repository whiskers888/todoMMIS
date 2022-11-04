using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Controllers
{
    public class BaseController: Controller
    {
        public ApplicationContext ApplicationContext { get; }

        public BaseController(ApplicationContext appContext)
        {
            ApplicationContext = appContext;
        }
        internal dynamic GetCommon()
        {
            dynamic common = new ExpandoObject();

            return common;
        }

        internal JsonResult Send(bool status,object data)
        {
            return Json(new Answer()
            {
                Status = status,
                Data = data,
                Datetime = DateTime.Now.ToString("u"),
            }); ; ;
        }
    }
}
