using Microsoft.EntityFrameworkCore;
using todoMMIS.Models.EF;

namespace todoMMIS.Contexts
{
    public class DBContext : DbContext
    {
        public DbSet<EFTodo> Todos { get; set; }
        public DbSet<EFUser> Users { get; set; }
        /*public DbSet<EFToken> Token { get; set; }*/
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