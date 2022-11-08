using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;
using XAct.Users;

namespace todoMMIS.Managers
{
    public class UsersManager : BaseManager<UserReplicate, EFUser>
    {
        private List<UserReplicate> Users { get; set; }
        public  List<string> Tokens { get; set; }
        public UsersManager(ApplicationContext appContext) : base(appContext)
        {
            Users = new List<UserReplicate>();
            Tokens = new List<string>();
        }

        public override void Read()
        {
            base.Read();
            foreach (UserReplicate replicate in replicates)
            {
                if (replicate.Token != null)
                {
                    Tokens.Add(replicate.Token);
                }
            }
        }

        public UserReplicate? Authorize(string login, string password, bool remember = false)
        {
            try
            {
                UserReplicate user = Items.FirstOrDefault(x => x.Username == login & x.Password == AppContext.GetHash(password));
                if (user != null)
                {
                    user.Token = AppContext.GenerateToken(user.Username);
                    AddToken(user.Token);
                    DBContext.SaveChanges();
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
                    AddToken(User.Token);


                    return base.Create(User);
                }
                return null;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }
        public void DeleteAuthorize(string access_token)
        {
            UserReplicate user = GetUser(access_token);
            Tokens.Remove(access_token);
            user.Context.Token = null;
            Update(user.Context);


        }
        public void AddToken(string Token)
        {
            Tokens.Add(Token);
        }
        public string FindToken(string Token)
        {
            string token = Tokens.FirstOrDefault(x => x == Token);
            return token != null ? token : null;
        }
        public void DeleteToken(string Token)
        {
            string token = Tokens.FirstOrDefault(x => x == Token);
            UserReplicate user = GetUser(token);
            user.Token = null;
            DBContext.Entry(user).State = EntityState.Modified;
                DBContext.SaveChanges();
        }

        public UserReplicate GetUser(string token)
        {
            UserReplicate User = replicates.FirstOrDefault(User => User.Token == token);
            return User;
        } 

    }
}
