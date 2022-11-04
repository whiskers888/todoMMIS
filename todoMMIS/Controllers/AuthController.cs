using Microsoft.AspNetCore.Mvc;
using todoMMIS.Contexts;
using todoMMIS.Replicates;

namespace todoMMIS.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(ApplicationContext _appContext) : base(_appContext) { }


        [HttpPost("[controller]/[action]")]
        public JsonResult SignIn([FromBody] string login, string password, bool remember = false)
        {
            var res = GetCommon();
            UserReplicate user = ApplicationContext.UserManager.Authorize(login, password, remember);
            res.item = user;
            return Send(true, res);
        }

        [HttpPost("[controller]/[action]")]
        public JsonResult SignUp([FromBody] object data)
        {
            UserReplicate user = ApplicationContext.UserManager.Create(data);
            var res = GetCommon();
            if(user != null)
            {
                res.item = user;
                res.status = true;
            }
            else
            {
                res.item = "Ошибка создания аккаунта";
                res.status = false;
            }
            
            return Send(res.status, res);
        }

    }
}
