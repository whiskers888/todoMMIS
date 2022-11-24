using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;
using XAct.Users;

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
        internal string GetToken()
        {
            return HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7);
        }

        internal JsonResult Exception( Exception ex) 
        {
            dynamic res = GetCommon();
            res.Exception = ex.Message;
            if(ex.InnerException != null)
            {
                res.InnerException = ex.InnerException.Message;
            }
            return Send(false, res);
        }

        internal JsonResult Execute(string token, Func< JsonResult> action, string expMessage)
        {
            TokenReplicate Token = ApplicationContext.TokensManager.replicates.FirstOrDefault(x => x.Token == token);
            try { return action(); }
            catch { return Send(false, expMessage); }


           
        }
    }
}
