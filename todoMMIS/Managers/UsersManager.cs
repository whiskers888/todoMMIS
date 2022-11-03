using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class UsersManager:BaseManager<UserReplicate, EFUser>
    {
        private List<UserReplicate> Users { get; set; }
        public UsersManager(ApplicationContext appContext) : base(appContext)
        {
            Users = new List<UserReplicate>();
        }

        public UserReplicate? Authorize(string login, string password)
        {
            return null;
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

        public UserReplicate GetUser(string token)
        {
           return Users.FirstOrDefault(user => user.Token == token);
        }

    }
}
