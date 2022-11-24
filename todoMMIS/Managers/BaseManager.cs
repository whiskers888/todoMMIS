using Microsoft.EntityFrameworkCore;
using todoMMIS.Models;
using todoMMIS.Contexts;
using todoMMIS.Replicates;
using XAct.Users;

namespace todoMMIS.Managers
{
    public class BaseManager<TReplicate, TModel> where TModel : EFBaseModel where TReplicate : BaseReplicate
    {
        protected ApplicationContext AppContext { get; }
        protected DBContext DBContext { get; }
        public BaseManager(ApplicationContext app)
        {
            AppContext = app;
            DBContext = app.CreateDBContext();
            replicates = new List<TReplicate>();
            Read();
        }

        public readonly List<TReplicate> replicates;

        public virtual void Read()
        {
            foreach (var prop in DBContext.GetType().GetProperties())
            {
                if (prop.GetValue(DBContext) is DbSet<TModel> items)
                {
                    foreach (var item in items.ToArray())
                    {
                        if(item.IsDeleted == false )
                        {
                            replicates.Add((TReplicate)Activator.CreateInstance(typeof(TReplicate), AppContext, item));
                        }
                    }
                }
            }
        }

        public TReplicate[] Items => replicates.ToArray();

        public virtual TReplicate? Create(TModel EFModel)
        {
            try
            {

                TReplicate replicate = (TReplicate)Activator.CreateInstance(typeof(TReplicate), AppContext, EFModel);

                //Добавляем репликейт в свой список, а модель в БД и сохраняем
                replicates.Add(replicate);
                DBContext.Add(EFModel);
                DBContext.SaveChanges();
                return replicate;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public TReplicate Update(TModel model)
        {
            try
            {
                // Обновляем репликейт
                TReplicate replicate = Get(model.Id);
                replicate.Update(model);


                DBContext.Entry(model).State = EntityState.Modified;
                DBContext.SaveChanges();
                return replicate;

            }catch(Exception ex)
            {
                throw;
            }
        }

        public TReplicate Update(TModel model, TReplicate replicate)
        {
            try
            {
                // Обновляем репликейт
                replicate.Update(model);


                DBContext.Entry(model).State = EntityState.Modified;
                DBContext.SaveChanges();
                return replicate;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public TReplicate? Get (int id)
        {
            try
            {
                foreach (TReplicate replicate in Items)
                {
                    if (replicate.Id == id && replicate.IsDeleted == false)
                    {
                        return replicate;
                    }
                }
                return null;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
           
        }

        public List<TReplicate>? GetAll(int id)
        {
            try
            {
                List<TReplicate> ListReplicate = new List<TReplicate>();
                foreach(TReplicate replicate in Items)
                {
                    if (replicate.Id == id && replicate.IsDeleted == false)
                    {
                        ListReplicate.Add(replicate);
                    }
                }
                return ListReplicate;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public virtual TReplicate Delete(int id)
        {
            try
            {
                TReplicate model = Get(id);
                model.Context.IsDeleted = true;
                Update((TModel)model.Context);
                replicates.Remove(model);
                return model;
            }catch(Exception ex)
            {
                throw;
            }
        }

    }
}
