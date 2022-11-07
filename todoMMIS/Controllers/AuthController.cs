using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Controllers
{
    public class AuthController : BaseController
    {

        public AuthController(ApplicationContext _appContext) : base(_appContext) { }


        [HttpPost("[controller]/[action]")]
        public JsonResult SignIn([FromBody] dynamic data, bool remember)
        {
            try
            {
                dynamic userData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(data.ToString());
                UserReplicate user = ApplicationContext.UserManager.Authorize(userData["Username"].ToString(), userData["Password"].ToString(), Convert.ToBoolean(userData["Remember"]));
                var res = GetCommon();
                res.item = user;
                return Send(true, res);
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

        [HttpPost("[controller]/[action]")]
        public JsonResult SignUp([FromBody] EFUser data)
        {
            try
            {
                UserReplicate user = ApplicationContext.UserManager.Create(data);
                var res = GetCommon();  
                bool status;
                if (user != null)
                {
                    res.item = user;
                    status = true;
                }
                else
                {
                    res.msg = "Ошибка создания аккаунта";
                    status = false;
                }

                return Send(status, res);

            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

        [Authorize]
        [HttpGet("[controller]/[action]")]
        public JsonResult SignOut()
        {
            try
            {
                string access_token = HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7);
                var user = ApplicationContext.UserManager.GetUser(access_token);
                ApplicationContext.UserManager.RemoveUser(user);
                var res = GetCommon();
                return Send(true, res);
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

    }
}
