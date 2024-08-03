using DataBase.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata;

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
            //Comment during updatnig db !!!
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("DataSource=./PolesDB.db");
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=PolesData;AttachDbFilename=D:\\NET_PC\\2\\PolesDB\\PolesDB\\db.mdf;Integrated Security=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employment>(entity =>
            {
                entity.OwnsOne(e => e.Contract, navigationBuilder =>
                {
                    navigationBuilder.Property(e => e.Value)
                                     .HasColumnName("Contract");
                });
            });
            modelBuilder.Entity<Person>(entity =>
            {
                entity.OwnsOne(p => p.Gender, navigationBuilder =>
                {
                    navigationBuilder.Property(p => p.Value)
                                     .HasColumnName("Gender");
                });
                entity.HasOne(p => p.Parent)
                    .WithMany(p => p.Children);
                entity.HasOne(p => p.Partner)
                    .WithOne()
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.Employments)
                    .WithOne(e => e.Emploee)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasMany(e => e.Employed)
                    .WithOne(e => e.Company);
            });
        }
    }
}
