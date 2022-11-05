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
            var user = ApplicationContext.DecodeToken(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
            var todos= ApplicationContext.TodoManager.GetAll(user);
            dynamic res = GetCommon();
            res.items = todos;
            return Send(true, res);
        }

        [Authorize]
        [HttpPost("[controller]/[action]")]
        public JsonResult Get([FromBody] dynamic data)
        {
            dynamic Data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(data.ToString());
            var user = ApplicationContext.DecodeToken(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
            var todos = ApplicationContext.TodoManager.Get(Convert.ToUInt64(Data["Id"]), user);
            dynamic res = GetCommon();
            res.items = todos;
            return Send(true, res);
        }
    }
}
