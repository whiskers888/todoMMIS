using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Text;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;
using XAct;

namespace todoMMIS.Managers
{
    public class UsersManager : BaseManager<UserReplicate, EFUser>
    {
        public UsersManager(ApplicationContext appContext) : base(appContext) { }

        public UserReplicate? Authorize(string login, string password)
        {
            try
            {
                UserReplicate User = replicates.FirstOrDefault(x => x.Username == login & x.Password == AppContext.GetHash(password));
                if (User != null)
                {
                    User.Token = AppContext.GenerateToken(User.Username);

                    EFToken token = AppContext.TokensManager.Create(new EFToken()
                    {
                        Token = User.Token,
                        User = User.Username,
                        IsDeleted = false,
                    });
                    DBContext.Add(token);

                    DBContext.SaveChanges();
                    return User;
                }
                return null;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }
        }
        public override UserReplicate? Create(EFUser User)
        {
            try
            {
                if (replicates.FirstOrDefault(user => user.Username == User.Username) == null)
                {
                    User.Password = AppContext.GetHash(User.Password);
                    User.Token = AppContext.GenerateToken(User.Username);
                    User.IsDeleted = false;

                    UserReplicate replicateUser = (UserReplicate)Activator.CreateInstance(typeof(UserReplicate), AppContext, User);

                    replicates.Add(replicateUser);
                    DBContext.Add(User);

                    EFToken token = AppContext.TokensManager.Create(new EFToken()
                    {
                        Token = User.Token,
                        User = User.Username,
                        IsDeleted = false,
                    });
                    DBContext.Add(token);

                    DBContext.SaveChanges();
                    return replicateUser;
                }
                return null;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }

        public override UserReplicate Delete(int id)
        {
            UserReplicate user = Get(id);
            AppContext.TokensManager.Delete(user.Token);
            user.Token = null;

            Update(user.Context);

            return base.Delete(id);
        }

        public void DeleteToken(string token)
        {
            UserReplicate user = replicates.FirstOrDefault(x => x.Token == token);
            AppContext.TokensManager.Delete(user.Token);
            user.Token = null;
            DBContext.Entry(user.Context).State = EntityState.Modified;
            DBContext.SaveChanges();
        }

        public UserReplicate GetUser(string token)
        {
            UserReplicate? User = replicates.FirstOrDefault(User => User.Token == token);
            return User;
        }
    }
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new(Encoding.UTF8.GetBytes(KEY));
    }
}
