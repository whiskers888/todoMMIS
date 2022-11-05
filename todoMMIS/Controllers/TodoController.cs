using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoMMIS.Contexts;

namespace todoMMIS.Controllers
{
    public class TodoController : BaseController
    {
        public TodoController(ApplicationContext appContext) : base(appContext) { }

        [Authorize]
        [HttpGet("[controller]/[action]")]
        public JsonResult Test()
        {
            dynamic res = GetCommon();
            res.items = ApplicationContext.TodoManager.Items;
            return Send(true, res);
        }

        [Authorize]
        [HttpGet("[controller]/[action]")]
        public JsonResult GetAll()
        {
            string access_token = HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7);
            var user = ApplicationContext.DecodeToken(access_token);

            dynamic res = GetCommon();
            res.items = user;
            return Send(true, res);
        }
    }
}
