using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Controllers
{
    public class AuthController : BaseController
    {

        public AuthController(ApplicationContext _appContext) : base(_appContext) { }


        [HttpPost("[controller]/[action]")]
        public JsonResult SignIn([FromBody] EFUser data)
        {
            try
            {
                
                    UserReplicate user = ApplicationContext.UserManager.Authorize(data.Username, data.Password);
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
                UserReplicate User = ApplicationContext.UserManager.Create(data);
                var res = GetCommon();  
                bool status;
                if (User != null)
                {
                    res.item = User;
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
        public new JsonResult SignOut()
        {
            try
            {
                return Execute(GetToken(),() =>
                {
                    string access_token = HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7);
                    var user = ApplicationContext.UserManager.GetUser(access_token);
                    ApplicationContext.UserManager.DeleteToken(access_token);
                    var res = GetCommon();
                    res.msg = "Goodbye";
                    return Send(true, res);
                }, "Token is invalid");
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

    }
}
