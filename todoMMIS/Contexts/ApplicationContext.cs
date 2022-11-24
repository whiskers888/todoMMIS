using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
            TodoManager = new TodosManager(this);
            UserManager = new UsersManager(this);
/*            TokensManager = new TokensManager(this);*/
        }

        public TodosManager TodoManager { get; set; }
        public UsersManager UserManager { get; set; }
/*        public TokensManager TokensManager { get; set; }*/

        public string Version { get; }

        public string Title { get; }

        public IConfiguration Config { get; }

        public DBContext CreateDBContext()
        {
            return new DBContext(Config.GetConnectionString("DefaultConnection"));
        }

        public string GetHash(string input)
        {
            var tmpSource = Encoding.UTF8.GetBytes(input);
            var hash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return Convert.ToBase64String(hash);
        }
        public string GenerateToken(string username)
        {
            var handler = new JwtSecurityTokenHandler();
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Expired, DateTime.Now.AddDays(7).ToString())};
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10080)), // время действия 1 неделя
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return handler.WriteToken(jwt);
        }

        public Dictionary<string, string> DecodeToken(string token)
        {
            Dictionary<string, string> answer = new Dictionary<string, string>();
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = AuthOptions.AUDIENCE,
            };
            var claims = handler.ValidateToken(token, validations, out var _);
            answer.Add("User", claims.Identity.Name);
            answer.Add("ExpireDate", claims.Identity.Expired);
            return claims.Identity.Name;
        }
    }
    
}
