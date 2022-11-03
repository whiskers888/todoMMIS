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
                    // Если ID невозможно поменять надо проверить на геттеры и сеттеры
                    GetType().GetField(key.ToString()).SetValue(this,model[key]);
                    return true;
                }
            }
            return false;
        }
    }
}
