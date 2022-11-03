using System.Reflection;
using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Replicates
{
    public class BaseReplicate
    {
        public ApplicationContext AppContext { get; set; }
        public EFBaseModel Context { get; set; }
        public int Id { get;  }
        public bool IsAvailable { get; }

        public BaseReplicate(ApplicationContext appContext, EFBaseModel context)
        {
            AppContext = appContext;
            Context = context;
            IsAvailable = true;
        }
        public bool Update(Dictionary<string, dynamic> model)
        {
            bool hasChanges = false;
            foreach( var pair in model){
                foreach(var item in GetType().GetFields()) {
                    if( item.Name == pair.Key & GetType().GetField(pair.Key) != pair.Value){
                        hasChanges = true;
                        GetType().GetProperty(pair.Key).SetValue(pair.Key, pair.Value);
                    }
                }
            }
            AppContext.CreateDbContext().SaveChanges();
            return hasChanges;
        }
        public void Delete()
        {
            //???
            AppContext.CreateDbContext().Remove(this);
        }
        public void Save(){
            AppContext.CreateDbContext().SaveChanges();
        }


    }
}
