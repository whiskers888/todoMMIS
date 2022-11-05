using Microsoft.EntityFrameworkCore;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;
using XAct.Users;

namespace todoMMIS.Managers
{
    public class UsersManager:BaseManager<UserReplicate, EFUser>
    {
        private List<UserReplicate> Users { get; set; }
        public UsersManager(ApplicationContext appContext) : base(appContext)
        {
            Users = new List<UserReplicate>();
            Read();
        }

        public UserReplicate? Authorize(string login, string password, bool remember = false)
        {
            try
            {
                EFUser user = DBContext.Users.FirstOrDefault(x => x.Username == login & x.Password == AppContext.GetHash(password));
                if (user != null )
                {
                    user.Token = AppContext.GenerateToken(user.Username);
                    DBContext.SaveChanges();
                    UserReplicate replicate = new(AppContext, user);
                    if (remember)
                    {
                        AddUser(replicate);
                    }
                     return replicate;
                }
                return null;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }
        public override UserReplicate? Create(dynamic model)
        {
            try
            {
                EFUser EFUser = Newtonsoft.Json.JsonConvert.DeserializeObject<EFUser>(model.ToString());
                UserReplicate replicate = (UserReplicate)Activator.CreateInstance(typeof(UserReplicate), AppContext, EFUser);

                if (replicates.FirstOrDefault(user => user.Username == replicate.Username) == null)
                {
                    EFUser.Password = AppContext.GetHash(EFUser.Password);
                    EFUser.Token = AppContext.GenerateToken(EFUser.Username);
                    AddUser(replicate);
                    //Добавляем репликейт в свой список, а модель в БД и сохраняем
                    replicates.Add(replicate);
                    DBContext.Add(EFUser);
                    DBContext.SaveChanges();
                    return replicate;
                }
                return null;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }
        public void AddUser(UserReplicate user)
        {
            if (Users.Contains(user) == false)
            {
                _ = Users.Append(user);
            }
        }

        public void RemoveUser(UserReplicate user)
        {
            if (Users.Contains(user))
                Users.Remove(user);
        }

        public UserReplicate GetUser(string token) => Users.FirstOrDefault(user => user.Token == token);

    }
}
