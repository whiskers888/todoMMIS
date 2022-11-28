using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using todoMMIS.Contexts;
using todoMMIS.Models.Answer;
using todoMMIS.Models.EF;
using todoMMIS.Replicates;

namespace todoMMIS.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController (ApplicationContext _appContext) : base(_appContext) { }

        
        [HttpGet("[controller]/[action]")]
        public JsonResult GetInfo()
        {
            try
            {
                return Execute((UserReplicate User) =>
                {
                    var res = GetCommon();
                    res.user = User;
                    return Send(true, res);
                }, "Token is Invalid");
            }
            catch(Exception ex)
            {
                return Exception(ex);
            }
    
        }

        
        [HttpPost("[controller]/[action]")]
        public JsonResult Update([FromBody] EFUser data)
        {
            try
            {
                return Execute((User) =>
                {
                    data.Id = User.Id;

                    UserReplicate UpdateData = ApplicationContext.UserManager.Update(data);

                    dynamic res = GetCommon();
                    res.item = UpdateData;
                    return Send(true, res);
                }, "Token is Invalid");
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

        [HttpPost("[controller]/[action]")]
        public JsonResult ChangePass([FromBody] ChangePassModel data)
        {
            try
            {
                return Execute((User) =>
                {
                    ApplicationContext.UserManager.ChangePassword(data,User);
                    return Send(true, "Пароль сменен");
                }, "Token is Invalid");
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }

        }


        [HttpGet("[controller]/[action]")]
        public JsonResult Delete()
        {
            try
            {
                return Execute((User) =>
                {
                    ApplicationContext.UserManager.Delete(User);
                    return Send(true, "Пользователь удален");
                }, "Token is Invalid");
            }
            catch(Exception ex)
            {
                return Exception(ex);
            }
            
        }


    }
}
