using todoMMIS.Models;

namespace todoMMIS.Contexts
{
    public class UserContext
    {
        public ApplicationContext AppContext { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public string Token { get; set; }

        public UserContext(ApplicationContext _appContext, EFUser context)
        {
            AppContext = _appContext;
            FirstName = context.FirstName;
            LastName = context.LastName;
            Patronymic = context.Patronymic;
            Username = context.Username;
            Email = context.Email;
            Hash = context.Hash;
            Token = context.Token;

        }
    }
}
