using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using todoMMIS.Contexts;
using todoMMIS.Models;
using todoMMIS.Replicates;

namespace todoMMIS.Managers
{
    public class BaseManager<TReplicate, TModel> where TModel : EFBaseModel where TReplicate : BaseReplicate
    {
        public ApplicationContext AppContext { get;  }
        public TReplicate Replicate { get; set; }
        public TModel Model { get; set; }
        public List<BaseReplicate> Items { get; set; } 
        public List<BaseReplicate> UpdateItems { get; set; }
        public bool hasUpdates { get; set; }

        public BaseManager(ApplicationContext appContext)
        {
            AppContext = appContext;
            Items = new List<BaseReplicate> ();
            UpdateItems = new List<BaseReplicate> { };
            hasUpdates = false;
            Read();

        }

        public void Read()
        {
            Items?.Clear();
            /* При переносе сделать через DBContext*/
            foreach (EFBaseModel item in Model.GetType().GetFields().Select(field => field.GetValue(Model)))
            {
                Items?.Add(new BaseReplicate(AppContext, item));
            };
        }

        public BaseReplicate? Create(Dictionary<string, dynamic> model)
        {
            BaseReplicate item = new( AppContext, GetObject<EFBaseModel>(model));
            Items.Add(item);
            hasUpdates = true;
            return item;
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
                BaseReplicate? _item = Get(id);
                foreach (BaseReplicate item in Items){
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

        public List<BaseReplicate> GetUpdated(bool include_unavailable = false)
        {
            List<BaseReplicate> Result = null;
            foreach (BaseReplicate item in UpdateItems)
            {
                if (item.IsAvailable & include_unavailable)
                {
                    Result?.Append(item);
                }
            }
            UpdateItems.Clear();
            return Result;
        }

        public List <BaseReplicate> GetAll()
        {
            return Items;
        }

        // Я вроде знаю реализацию этой ебучей сериализации но пока в падлу ибо не протестить, чуть позже сделать!!!!!!!
        public List <BaseReplicate> GetJson( bool reverse = true)
        {
            return null;
        }
        //

        public List<BaseReplicate> GetAvaliable()
        {
            List<BaseReplicate> Result = null;
            foreach(BaseReplicate item in Items){
                if (item.IsAvailable){
                    Result.Append(item);
                }
            }
            return Result;
        }
        public void Save()
        {
            foreach( BaseReplicate item in Items)
            {
                item.Save();
            }
        }
        
        static T GetObject<T>(Dictionary<string, dynamic> dict)
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
