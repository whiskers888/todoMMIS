using Microsoft.AspNetCore.Mvc;
using todoMMIS.Contexts;
using todoMMIS.Replicates;

namespace todoMMIS.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(ApplicationContext _appContext) : base(_appContext) { }


        [HttpPost("[controller]/[action]")]
        public JsonResult SignIn(string login, string password, bool remember = false)
        {
            var res = GetCommon();
            UserReplicate user = ApplicationContext.UserManager.Authorize(login, password, remember);
            res.item = user;
            return Send(true, res);
        }

        [HttpPost("[controller]/[action]")]
        public JsonResult SignUp(dynamic request)
        {
            ApplicationContext.UserManager.Create(request);
            var res = GetCommon();
            
            return Send(true, res);
        }

    }
}
