using todoMMIS.Contexts;
using todoMMIS.Models;

namespace todoMMIS.Replicates
{
    public class BaseReplicate
    {
        EFBaseModel Context { get; set; }
        protected ApplicationContext App { get; }
        public BaseReplicate(ApplicationContext _app, EFBaseModel _context)
        {
            App = _app;
            Context = _context;
        }
        public int Id => Context.Id;
        public bool IsDeleted
        {
            get => Context.IsDeleted;
            set => Context.IsDeleted = value;
        }

        public bool Update(dynamic model)
        {
            foreach(var key in GetType().GetFields())
            {
                if (model[key] != null && GetType().GetField(key.ToString()).GetValue(this) != model[key])
                {
                    // Если ID будет пробовать поменять то надо проверить поле на сеттеры
                    GetType().GetField(key.ToString()).SetValue(this,model[key]);
                    return true;
                }
            }
            return false;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
