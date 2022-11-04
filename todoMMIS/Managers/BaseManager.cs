﻿using Microsoft.EntityFrameworkCore;
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
        protected ApplicationContext AppContext { get; }
        protected DBContext DBContext { get; }
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

        public bool Create(dynamic model)
        {
            try
            {
                // Получаем модель и репликейт из JSON
                TModel EFModel = JsonConvert.DeserializeObject<TModel>(model);
                TReplicate replicate = (TReplicate)Activator.CreateInstance(typeof(TReplicate), AppContext, EFModel);
                
                //Добавляем репликейт в свой список, а модель в БД и сохраняем
                replicates.Add(replicate);
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
                // Обновляем репликейт
                TReplicate replicate = Get(model["id"]);
                replicate.Update(model);

                // Обновляем модель в БД
                TModel EFModel = JsonConvert.DeserializeObject<TModel>(model);
                DBContext.Entry(EFModel).State = EntityState.Modified;
                DBContext.SaveChanges();
                return true;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public TReplicate? Get (int id)
        {
            try
            {
                foreach (TReplicate replicate in Items)
                {
                    if (replicate.Id == id)
                    {
                        return replicate;
                    }
                }
                return null;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }

        public TReplicate[]? GetAll(int id)
        {
            try
            {
                return Items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public bool Delete(dynamic model)
        {
            try
            {
                TReplicate? replicate = Get(model["id"]);
                replicate?.Delete();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
