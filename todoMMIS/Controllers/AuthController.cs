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
            return Execute((db) =>
            {
                dynamic result = GetCommon();

                EFUser user = db.User.FirstOrDefault(user => user.Username == login && user.Hash == _appContext.GetHash(password));

                if (user != null)
                {
                    user.Hash = _appContext.GetHash(Guid.NewGuid().ToString());
                    db.SaveChanges();

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
