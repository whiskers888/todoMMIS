using System;
using System.Collections.Generic;
using System.Linq;
using todoMMIS.Contexts;
using todoMMIS.Models.Answer;
using todoMMIS.Models.EF;
using todoMMIS.Replicates;
using XAct.Users;

namespace todoMMIS.Managers
{
    public class UsersManager : BaseManager<UserReplicate, EFUser>
    {
        public  List<string> Tokens { get; set; }
        public UsersManager (ApplicationContext appContext) : base(appContext) { }

        public override void Read()
        {
            Tokens = new List<string>();
            base.Read();
            foreach (UserReplicate replicate in replicates)
            {
                if (replicate.Token != null)
                {
                    Tokens.Add(replicate.Token);
                }
            }
        }

        public UserReplicate? Authorize(string login, string password)
        {
            try
            {
                UserReplicate user = replicates.FirstOrDefault(x => x.Username == login & x.Password == AppContext.GetHash(password));
                if (user != null)
                {
                    user.Token = AppContext.GenerateToken(user.Username);
                    AddToken(user.Token);
                    DBContext.SaveChanges();
                    return user;
                }
                return null;
            }catch(Exception ex)
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
            }catch(Exception ex)
            {
                throw;
            }
        }

        public override UserReplicate Update(EFUser NewModel)
        {
            try
            {
                UserReplicate replicate = Get(NewModel.Id);
                NewModel.Username = replicate.Username;
                NewModel.IsDeleted = replicate.IsDeleted;
                NewModel.Token = replicate.Token;
                NewModel.Password = replicate.Password;
                return base.Update(NewModel);
            }
            catch
            {
                throw;
            }
        }
        public void ChangePassword(ChangePassModel data, UserReplicate user)
        {
            if (user.Password == AppContext.GetHash(data.OldPassword))
            {
                user.Context.Password = AppContext.GetHash(data.NewPassword);
                base.Update(user.Context);
            }
        }

        public void DeleteAuthorize(string access_token)
        {
            try
            {
                UserReplicate user = GetUser(access_token);
                Tokens.Remove(access_token);
                user.Context.Token = null;
                base.Update(user.Context);
            }
            catch
            {
                throw;
            }
        }
        public void AddToken(string Token)
        {
            Tokens.Add(Token);
        }

        public string FindToken(string Token)
        {
            return Tokens.FirstOrDefault(x => x == Token);
        }

        public UserReplicate GetUser(string token)
        {
            return replicates.FirstOrDefault(User => User.Token == token);
        } 

    }
}
