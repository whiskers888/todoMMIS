﻿using Microsoft.EntityFrameworkCore;
using WebUI.EFModels;

namespace WebUI.Contexts
{
    public class DBContext : DbContext
    {
        public DbSet<EFTodo> Todos { get; set; }

        public DBContext(string cnnString)
        {
            ConnectionString = cnnString;
/*            Database.EnsureDeleted();
            Database.EnsureCreated();*/
        }

        public string ConnectionString { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}