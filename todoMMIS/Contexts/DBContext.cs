using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using todoMMIS.Models;


namespace todoMMIS.Contexts
{
    public class DBContext: DbContext
    {
        public DbSet<EFTodo> TodoItems { get; set; }

        public DbSet<EFUser> User { get; set; }
        public DBContext(string cnnString)
        {
            ConnectionString = cnnString;
        }

        public string ConnectionString { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
