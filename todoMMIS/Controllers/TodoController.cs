using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Controllers
{
    [Authorize]
    public class TodoController : BaseController
    {

       
        public TodoController(ApplicationContext appContext) : base(appContext) { }

        

        [HttpPost("[controller]/[action]")]
        public JsonResult Create([FromBody] EFTodo data)
        {
            
            try
            {
                data.User = ApplicationContext.DecodeToken(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                TodoReplicate todos = ApplicationContext.TodoManager.Create(data);

                dynamic res = GetCommon();
                res.items = todos;
                res.msg = "Задача успешно создана";
                return Send(true, res);
            }catch(Exception ex)
            {
                return Exception(ex);
            }
        }
        [HttpGet("[controller]/[action]")]
        public JsonResult GetAll()
        {
            dynamic res = GetCommon();
            try
            {
                string user = ApplicationContext.DecodeToken(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                TodoReplicate[] todos = ApplicationContext.TodoManager.GetAll(user).ToArray();
                res.items = todos;
                return Send(true, res);
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }

        }

        [HttpPost("[controller]/[action]")]
        public JsonResult Get(int id)
        {
            try
            {
                string user = ApplicationContext.DecodeToken(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                TodoReplicate todos = ApplicationContext.TodoManager.Get(id, user);

                dynamic res = GetCommon();
                res.items = todos;
                return Send(true, res);

            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }

        [HttpPost("[controller]/[action]")]
        public JsonResult Update( [FromBody] EFTodo data)
        {
            try
            {
                data.User = ApplicationContext.DecodeToken(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                TodoReplicate UpdateData = ApplicationContext.TodoManager.Update(data);

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
        public JsonResult Delete(int id)
        {
            try
            {
                string user = ApplicationContext.DecodeToken(HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7));
                TodoReplicate replicate = ApplicationContext.TodoManager.Delete(id);

                dynamic res = GetCommon();
                if (replicate != null)
                {
                    res.msg = "Задача успешно удалена";
                    return Send(true, res);
                }
                else
                {
                    res.msg = "Задачи с таким ID не существует";
                    return Send(false, res);
                }
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }
    }
}
