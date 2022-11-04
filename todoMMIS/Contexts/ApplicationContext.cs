using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todoMMIS.Managers;
using XSystem.Security.Cryptography;

namespace todoMMIS.Contexts
{
    public class ApplicationContext
    {
        public ApplicationContext(IConfiguration config)
        {
            Version = "0.0.0.1";
            Title = "TodoMMIS";
            Config = config;
            Initialize();
        }

        public void Initialize()
        {
            TodoManager = new TodoManager(this);
            UserManager = new UsersManager(this);
        }

        public TodoManager TodoManager { get; set; }
        public UsersManager UserManager { get; set; }

        public string Version { get; }

        public string Title { get; }

        public IConfiguration Config { get; }

        public DBContext CreateDBContext()
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
            var hash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return Convert.ToBase64String(hash);
        }
    }
}
