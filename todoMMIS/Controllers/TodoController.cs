using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
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
                return Execute(GetToken(), (User) =>
                {
                    data.User = User.Username;
                    data.createdAt = DateTime.Now.ToLocalTime();
                    TodoReplicate todos = ApplicationContext.TodoManager.Create(data);

                    dynamic res = GetCommon();
                    res.items = todos;
                    res.msg = "Задача успешно создана";
                    return Send(true, res);
                }, "Token is Invalid");
            }
            catch(Exception ex)
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
                return Execute(GetToken(), (User) =>
                {
                    TodoReplicate[] todos = ApplicationContext.TodoManager.GetAll(User.Username).ToArray();
                    res.items = todos;
                    return Send(true, res);
                }, "Token is Invalid");
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
                return Execute(GetToken(), (User) =>
                {
                    TodoReplicate todos = ApplicationContext.TodoManager.Get(id, User.Username);
                    dynamic res = GetCommon();
                    res.items = todos;
                    return Send(true, res);
                }, "Token is Invalid");

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
                return Execute(GetToken(), (User) =>
                {
                    data.User = User.Username;

                    TodoReplicate UpdateData = ApplicationContext.TodoManager.Update(data);

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

        [HttpGet("[controller]/[action]")]
        public JsonResult Delete(int id)
        {
            try
            {
                return Execute(GetToken(), (UserReplicate User) =>
                {
                    TodoReplicate todo = ApplicationContext.TodoManager.Get(id);
                    TodoReplicate replicate = ApplicationContext.TodoManager.Delete(todo);

                    dynamic res = GetCommon();
                    res.msg = replicate != null ? "Задача успешно удалена" : "Задачи с таким ID не существует";
                    return Send(false, res);

                }, "Token is Invalid");
            }
            catch (Exception ex)
            {
                return Exception(ex);
            }
        }
    }
}
