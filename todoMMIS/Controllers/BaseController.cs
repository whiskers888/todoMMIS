using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;
using XAct;

namespace todoMMIS.Controllers
{
    public class BaseController : Controller
    {
        public ApplicationContext ApplicationContext { get; }
        public BaseController (ApplicationContext appContext)
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

        internal JsonResult Exception(Exception ex)
        {
            dynamic res = GetCommon();
            res.Exception = ex.Message;
            if (ex.InnerException != null)
            {
                res.InnerException = ex.InnerException.Message;
            }
            return Send(false, res);
        }
        internal string GetToken()
        {
            return HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7);
        }

        internal JsonResult Execute(Func<string, JsonResult> action, string expMessage)
        {
            string token = ApplicationContext.UserManager.FindToken(GetToken());
            if (token != null)
            {
                return action(token);
            } 
            return Send(false, expMessage);
        }

        internal JsonResult Execute(Func< UserReplicate, JsonResult> action, string expMessage)
        {
            string token = ApplicationContext.UserManager.FindToken(GetToken());
            if (token != null)
            {
                UserReplicate User = ApplicationContext.UserManager.GetUser(token);
                return action(User);
            }
            return Send(false, expMessage);
        }
    }
}
