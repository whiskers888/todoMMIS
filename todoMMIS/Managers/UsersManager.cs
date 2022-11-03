using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class UsersManager
    {
        private ApplicationContext AppContext { get; }
        private List<UserContext> Users { get; set; }
        public UsersManager(ApplicationContext appContext)
        {
            AppContext = appContext;
            Users = new List<UserContext>();
        }

        public UserContext? Authorize(string login, string password)
        {
            DBContext db = AppContext.CreateDbContext();
            EFUser user = db.User.FirstOrDefault(x => x.Username == login & x.Hash == AppContext.GetHash(password));
            if (user != null)
            {
                user.Token = AppContext.GenerateToken();
                UserContext user_context = new(AppContext, user);
                AddUser(user_context);
                return user_context;
            }
            return null;
        }

        public void AddUser(UserContext user)
        {
            if (Users.Contains(user) == false)
            {
                _ = Users.Append(user);
            }
        }

        public void RemoveUser(UserContext user)
        {
            if (Users.Contains(user))
                Users.Remove(user);
        }

        public UserContext GetUser(string token)
        {
            return Users.FirstOrDefault(user => user.Token == token);
        }

    }
}
