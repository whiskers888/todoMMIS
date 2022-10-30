using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class BaseManager
    {
        public ApplicationContext AppContext { get;  }
        public BaseReplicate Replicate { get; set; }
        public EFBaseModel Model { get; set; }
        public List<BaseReplicate>? Items { get; set; } 
        public List<BaseReplicate> UpdateItems { get; set; }
        public bool hasUpdates { get; set; }

        public BaseManager(ApplicationContext appContext,  BaseReplicate replicate, EFBaseModel model )
        {
            this.AppContext = appContext;
            this.Replicate = replicate;
            this.Model = model;
            this.Items = new List<BaseReplicate> ();
            this.UpdateItems = new List<BaseReplicate> { };
            this.hasUpdates = false;
            this.Read();

        }

        public void Read()
        {
            this.Items?.Clear();
            foreach (EFBaseModel item in Model.GetType().GetFields().Select(field => field.GetValue(Model)))
            {
                this.Items.Add(new BaseReplicate(AppContext, item));
            };
        }

        public BaseReplicate? Create(Dictionary<string, dynamic> model)
        {
            try
            {
                BaseReplicate item = new BaseReplicate( AppContext, GetObject<EFBaseModel>(model));
                this.Items.Add(item);
                this.hasUpdates = true;
                return item;
            }
            catch
            {
                return null;
            }
        }

        public BaseReplicate? Update(Dictionary<string, dynamic> model, bool untracked = false)
        {
            try
            {
                BaseReplicate item = Get(model["Id"]);
                bool hasChanges = item.Update(model);
                if (!untracked & hasChanges)
                {
                    if (UpdateItems.Contains(item))
                    {
                        UpdateItems.Remove(item);
                    }
                    UpdateItems.Append(item);
                    hasUpdates = true;
                }
                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            
        }

        public bool Delete( int id){
            try{
                BaseReplicate _item = this.Get(id);
                foreach (BaseReplicate item in this.Items){
                    if (item.Context.Id == _item.Context.Id)
                    {
                        Items.Remove(item);
                    }
                }
                _item = null;
                hasUpdates = true;
                return true;
            }
            catch{
                return false;
            }
        }

        public BaseReplicate? Get(int id)
        {
            foreach (BaseReplicate item in Items)
            {
                if (item.Context.Id == id)
                {
                    return item;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public List<BaseReplicate> getUpdated(bool include_unavailable = false)
        {
            List<BaseReplicate> Result = null;
            foreach (BaseReplicate item in UpdateItems)
            {
                if (item.IsAvailable & include_unavailable)
                {
                    Result.Append(item);
                }
            }
            UpdateItems.Clear();
            return Result;
        }

        public List <BaseReplicate> getAll()
        {
            return Items;
        }

        // Я вроде знаю реализацию этой ебучей сериализации но пока в падлу ибо не протестить, чуть позже сделать!!!!!!!
        public List <BaseReplicate> GetJson( bool reverse = true)
        {
            List 
            return null;
        }
        //

        public List<BaseReplicate> GetAvaliable()
        {
            List<BaseReplicate> result = null;
            foreach(BaseReplicate item in Items){
                if (item.IsAvailable){
                    result.Append(item);
                }
            }
            return result;
        }
        public void save()
        {
            foreach( BaseReplicate item in Items)
            {
                item.Save();
            }
        }
        
        T GetObject<T>(Dictionary<string, dynamic> dict)
        {
            Type type = typeof(T);
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                type.GetProperty(kv.Key).SetValue(obj, kv.Value);
            }
            return (T)obj;
        }

    }
}
