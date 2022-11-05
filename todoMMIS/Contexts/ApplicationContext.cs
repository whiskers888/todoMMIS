using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using todoMMIS.Managers;
using XAct.Users;
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


        public string GenerateToken(string username)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10080)), // время действия 1 неделя
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GetHash(string input)
        {
            var tmpSource = Encoding.UTF8.GetBytes(input);
            var hash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return Convert.ToBase64String(hash);
        }
    }
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
