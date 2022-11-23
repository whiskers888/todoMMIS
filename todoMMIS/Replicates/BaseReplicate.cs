using System;
using todoMMIS.Contexts;
using todoMMIS.Models.EF;

namespace todoMMIS.Replicates
{
    public class BaseReplicate
    {
        internal EFBaseModel Context { get; set; }
        protected ApplicationContext App { get; }
        public BaseReplicate (ApplicationContext _app, EFBaseModel _context)
        {
            App = _app;
            Context = _context;
        }
        public int Id => Context.Id;
        public bool? IsDeleted
        {
            get => Context.IsDeleted;
            set => Context.IsDeleted = value;
        }

        public bool Update(dynamic model)
        {
            try
            {
                dynamic items = GetType().GetProperties();
                foreach (var key in items)
                {
                    dynamic field = model.GetType().GetProperty(key.Name).GetValue(model);
                    dynamic value = GetType().GetProperty(key.Name).GetValue(this);
                    if (field != null && value != field)
                    {
                        // Если ID будет пробовать поменять то надо проверить поле на сеттеры
                        GetType().GetProperty(key.Name).SetValue(this, field);
                    }
                }
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }
    }
}
