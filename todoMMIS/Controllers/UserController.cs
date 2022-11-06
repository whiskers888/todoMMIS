using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Controllers
{
    [Authorize]
    public class UserController:BaseController
    {
        public UserController(ApplicationContext _appContext) : base(_appContext) { }

        
        [HttpGet("[controller]/[action]")]
        public JsonResult GetInfo()
        {
            try
            {
                UserReplicate user = ApplicationContext.UserManager.GetUser(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                var res = GetCommon();
                res.user = user;
                return Send(true, res);
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
                data.Username = null;
                data.IsDeleted = null;
                data.Token = null;
                data.Password = null;

                UserReplicate user = ApplicationContext.UserManager.GetUser(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                data.Id = user.Id;
                UserReplicate UpdateData = ApplicationContext.UserManager.Update(data);
                dynamic res = GetCommon();
                res.item = UpdateData;
                return Send(true, res);
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
                UserReplicate user = ApplicationContext.UserManager.GetUser(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                ApplicationContext.UserManager.Delete(user.Id);
                return Send(true, "Пользователь удален");
            }
            catch(Exception ex)
            {
                return Exception(ex);
            }
            
        }


    }
}
