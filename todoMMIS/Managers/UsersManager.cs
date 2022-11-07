using Microsoft.EntityFrameworkCore;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class UsersManager : BaseManager<UserReplicate, EFUser>
    {
        private List<UserReplicate> Users { get; set; }
        public UsersManager(ApplicationContext appContext) : base(appContext)
        {
            Users = new List<UserReplicate>();
        }

        public UserReplicate? Authorize(string login, string password, bool remember = false)
        {
            try
            {
                UserReplicate user = Items.FirstOrDefault(x => x.Username == login & x.Password == AppContext.GetHash(password));
                if (user != null)
                {
                    user.Token = AppContext.GenerateToken(user.Username);
                    DBContext.SaveChanges();
                    if (remember)
                    {
                        AddUser(user);
                    }
                    return user;
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
                    

                    return base.Create(User);
                }
                return null;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }
        public void AddUser(UserReplicate user)
        {
            if (Users.Contains(user) == false)
            {
                Users.Add(user);    
            }
        }

        public void RemoveUser(UserReplicate user)
        {
            if (Users.Contains(user))
            {
                replicates.Remove(user);
                Users.Remove(user); 
                DeleteToken(user.Username);
            }  
        }

        public void DeleteToken(string username)
        {
            EFUser user = DBContext.Users.FirstOrDefault(x => x.Username == username);
            user.Token = null;
            DBContext.Entry(user).State = EntityState.Modified;
                DBContext.SaveChanges();
        }

        public UserReplicate GetUser(string token)
        {
            UserReplicate User = Items.FirstOrDefault(User => User.Token == token);
            return User;
        } 

    }
}
