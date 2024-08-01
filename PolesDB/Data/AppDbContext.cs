using DataBase.Model;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Data
{
    public class AppDbContext : DbContext
    {
        //EF tools commands (set WebApp as startup project):
        //Add-Migration InitialCreate
        //Update-Database
        //Drop-Database
        public DbSet<Person> Persons { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employment> Contracts { get; set; }

        public AppDbContext()
            : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=./PolesDB.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasOne(p => p.Parent)
                .WithMany(p => p.Children);
                entity.HasOne(p => p.Partner)
                .WithOne()
                .IsRequired(false);
                //entity.HasMany(p => p.ConnectionStartcity)
                //    .WithOne(d => d.StartCity)
                //    .HasForeignKey(d => d.StartCityId);

                //entity.HasMany(p => p.ConnectionEndCity)
                //    .WithOne(d => d.EndCity)
                //    .HasForeignKey(d => d.EndCityId);
            });
        }
    }
}
