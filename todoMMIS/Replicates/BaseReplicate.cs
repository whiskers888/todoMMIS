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
            this.AppContext = appContext;
            this.Context = context;
            this.IsAvailable = true;
        }
        public bool Update(Dictionary<string, dynamic> model)
        {
            bool hasChanges = false;
            foreach( var pair in model){
                foreach(var item in this.GetType().GetFields()) {
                    if( item.Name == pair.Key & this.GetType().GetField(pair.Key) != pair.Value){
                        hasChanges = true;
                        this.GetType().GetProperty(pair.Key).SetValue(pair.Key, pair.Value);
                    }
                }
            }
            this.AppContext.CreateDbContext().SaveChanges();
            return hasChanges;
        }
        public void Delete()
        {
            //???
            this.AppContext.CreateDbContext().Remove(this);
        }
        public void Save(){
            this.AppContext.CreateDbContext().SaveChanges();
        }


    }
}
