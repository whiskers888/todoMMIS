using Microsoft.AspNetCore.Mvc;
using System;
using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController (ApplicationContext _appContext) : base(_appContext) {   }
        [HttpPost("[controller]/[action]")]
        public JsonResult SignIn(string login, string password, bool remember = false)
        {
            return Execute((dbc) =>
            {
                dynamic result = GetCommon();

                EFUser user = dbc.User.FirstOrDefault(user => user.Username == login && user.Hash == _appContext.GetHash(password));

                if (user != null)
                {
                    user.Hash = _appContext.GetHash(Guid.NewGuid().ToString());
                    dbc.SaveChanges();

                    result.user = user.Username;
                    result.remember = remember;

                    _appContext.AddUser(user);

                    return Send("Успешная авторизация");
                }

                return Send("Неверные логин или пароль");

            }, "Ошибка авторизации");
        }

    }
}
