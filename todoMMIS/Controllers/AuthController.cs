using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
                return Execute(GetToken(), (access_token) =>
                {
                    ApplicationContext.UserManager.DeleteAuthorize(access_token);
                    var res = GetCommon();
                    return Send(true, res);

                }, "Token is Invalid");
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

    }
}
