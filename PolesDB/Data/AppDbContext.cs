using DataBase.Model;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employment> Contracts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=./PolesDB.db");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
