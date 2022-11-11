using Microsoft.EntityFrameworkCore;
using todoMMIS.Models;
using todoMMIS.Contexts;
using todoMMIS.Replicates;
using XAct.Users;
using System.Collections.Generic;
using System;
using System.Linq;

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
                    foreach (var item in  items.ToArray())
                    {
                        if(item.IsDeleted == false | item.IsDeleted == null)
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
                EFModel.IsDeleted = false;
                //Добавляем репликейт в свой список, а модель в БД и сохраняем
                DBContext.Add(EFModel);
                int a = DBContext.SaveChanges();

                TReplicate replicate = (TReplicate)Activator.CreateInstance(typeof(TReplicate), AppContext, EFModel);
                replicates.Add(replicate);

                return replicate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }
        }

        public virtual TReplicate Update(TModel model)
        {
            try
            {
                // Обновляем репликейт
                TReplicate replicate = Get(model.Id);
                replicate.Update(model);

                /*DBContext.Entry(model).State = EntityState.Modified;*/
                DBContext.SaveChanges();
                return replicate;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public TReplicate? Get (int id)
        {
            try
            {
                foreach (TReplicate replicate in Items)
                {
                    if (replicate.Id == id )
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

        public virtual TReplicate Delete(TReplicate replicate)
        {
            try
            {
                foreach(TReplicate item in replicates)
                {
                    if(item.Id == replicate.Id && item.IsDeleted == false)
                    {
                        item.Context.IsDeleted = true;
                        Update((TModel)item.Context);
                        replicates.Remove(item);
                        return item;
                    }
                }
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
