using System.Security.Cryptography;
using System.Text;
using todoMMIS.Managers;
using todoMMIS.Models;

namespace todoMMIS.Contexts
{
    public class ApplicationContext
    {
        public IConfiguration Config { get; }

        public UsersManager UserManager { get; }
        public TodoManager TodoManager { get;  }
        
        public ApplicationContext(IConfiguration config)
        {
            Config = config;
            TodoManager = new TodoManager(this);
            UserManager = new UsersManager(this);

        }

        public DBContext CreateDbContext()
        {
            return new DBContext(Config.GetConnectionString("DefaultConnection"));
        }

        public string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }

        public string GetHash(string input)
        {
            var tmpSource = Encoding.UTF8.GetBytes(input);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return Convert.ToBase64String(hash);
        }
        

    }
}
