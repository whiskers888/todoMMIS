using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUI.Contexts;

namespace WebUI.Managers
{
    public class BaseManager<TReplicate, TModel> where TModel : class
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

    }
}
