using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using todoMMIS.Models;
using todoMMIS.Contexts;
using todoMMIS.Replicates;
using Newtonsoft.Json;

namespace todoMMIS.Managers
{
    public class BaseManager<TReplicate, TModel> where TModel : EFBaseModel where TReplicate : BaseReplicate
    {
        ApplicationContext AppContext { get; }
        DBContext DBContext { get; }
        public BaseManager(ApplicationContext app)
        {
            AppContext = app;
            DBContext = app.CreateDBContext();
            replicates = new List<TReplicate>();
            Read();
        }

        private readonly List<TReplicate> replicates;

        private void Read()
        {
            foreach (var prop in DBContext.GetType().GetProperties())
            {
                if (prop.GetValue(DBContext) is DbSet<TModel> items)
                {
                    foreach (var item in items.ToArray())
                    {
                        replicates.Add((TReplicate)Activator.CreateInstance(typeof(TReplicate), AppContext, item));
                    }
                }
            }
        }

        public TReplicate[] Items => replicates.ToArray();
        
        public TReplicate ToReplicate(string model)
        {
            TModel EFModel = JsonConvert.DeserializeObject<TModel>(model);
            TReplicate item = (TReplicate)Activator.CreateInstance(typeof(TReplicate), AppContext, EFModel);
            return item;
        }

        public bool Create(string model)
        {
            try
            {
                TModel EFModel = JsonConvert.DeserializeObject<TModel>(model);
                TReplicate item = (TReplicate)Activator.CreateInstance(typeof(TReplicate), AppContext, EFModel);
        
                replicates.Add(item);
                DBContext.Add(EFModel);
                DBContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Update(dynamic model)
        {
            try
            {
                // GET Replicate by model["id"]
                // replicate.update(model)
                // 
                TModel EFModel = JsonConvert.DeserializeObject<TModel>(model);
                TReplicate replicate = (TReplicate)Activator.CreateInstance(typeof(TReplicate), AppContext, EFModel);
                DBContext.Entry(EFModel).State = EntityState.Modified;
                               
                return true;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
