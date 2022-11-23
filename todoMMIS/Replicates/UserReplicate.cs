using todoMMIS.Contexts;
using todoMMIS.Models.EF;

namespace todoMMIS.Replicates
{
    public class UserReplicate : BaseReplicate
    {
        internal EFUser Context { get; set; }
        public UserReplicate (ApplicationContext app, EFUser _context) : base(app, _context)
        {
            Context = _context;
        }

        public string Name
        {
            get => Context.Name;
            set => Context.Name = value;
        }
        public string Username
        {
            get => Context.Username;
            set => Context.Username = value;
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
