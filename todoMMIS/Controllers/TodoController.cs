﻿using Microsoft.AspNetCore.Mvc;
using todoMMIS.Contexts;

namespace todoMMIS.Controllers
{
    public class TodoController:BaseController
    {
        public TodoController(ApplicationContext appContext) : base(appContext){ }

        [HttpGet("[controller]/[action]")]
        public JsonResult Test()
        {
            dynamic res = GetCommon();
            res.items = ApplicationContext.TodoManager.Items;
            return Send(true, res);
        }
    }
}
