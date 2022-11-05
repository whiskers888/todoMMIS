using Microsoft.AspNetCore.Mvc;
using todoMMIS.Contexts;
using todoMMIS.Replicates;

namespace todoMMIS.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(ApplicationContext _appContext) : base(_appContext) { }


        [HttpPost("[controller]/[action]")]
        public JsonResult SignIn([FromBody] dynamic data)
        {
            dynamic userData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(data.ToString());
            UserReplicate user = ApplicationContext.UserManager.Authorize(userData["Username"].ToString(), userData["Password"].ToString());
            var res = GetCommon();
            res.item = user;
            return Send(true, res);
        }

        [HttpPost("[controller]/[action]")]
        public JsonResult SignUp([FromBody] object data)
        {
            UserReplicate user = ApplicationContext.UserManager.Create(data);
            var res = GetCommon();
            bool status = false;
            if(user != null)
            {
                res.item = user;
                status = true;
            }
            else
            {
                res.item = "Ошибка создания аккаунта";
                status = false;
            }
            
            return Send(status, res);
        }

    }
}
