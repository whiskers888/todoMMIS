using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Replicates
{
    public class UserReplicate : BaseReplicate
    {
        EFUser Context { get; set; }
        public UserReplicate(ApplicationContext app, EFUser _context) : base(app, _context)
        {
            Context = _context;
        }


        public string FirstName
        {
            get => Context.FirstName;
            set => Context.FirstName = value;
        }
        public string LastName
        {
            get => Context.LastName;
            set => Context.LastName = value;
        }
        public string Patronymic
        {
            get => Context.Patronymic;
            set => Context.Patronymic = value;
        }
        public string Username
        {
            get => Context.Username;
            set => Context.Username = value;
        }
        public string Email
        {
            get => Context.Email;
            set => Context.Email = value;
        }
        public string Password
        {
            get => Context.Password;
            set => Context.Password = value;
        }
        public string? Token
        {
            get => Context.Token;
            set => Context.Token = value;
        }

    }
}
