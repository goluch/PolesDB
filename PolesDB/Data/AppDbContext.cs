﻿using DataBase.Model;
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
        public DbSet<Employment> Employments { get; set; }

        private readonly StreamWriter _logStream = new StreamWriter("mylog.txt", append: true);

        public AppDbContext()
            : base()
        {
            //Comment this during updatnig db usung ef tools!!!
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("DataSource=./PolesDB.db");
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=PolesData;AttachDbFilename=D:\\NET_PC\\2\\PolesDB\\PolesDB\\db.mdf;Integrated Security=True");
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine)
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Query.Name })
                .EnableSensitiveDataLogging();
            //optionsBuilder.LogTo(_logStream.WriteLine)
            //    .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employment>(entity =>
            {
                entity.OwnsOne(e => e.Contract, navigationBuilder =>
                {
                    navigationBuilder.Property(e => e.ContractType)
                                     .HasColumnName("Contract");
                });
            });
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.OwnsOne(e => e.Gender, navigationBuilder =>
                {
                    navigationBuilder.Property(p => p.Value)
                                     .HasColumnName("Gender");
                });
                entity.Ignore(p => p.Children);
                entity.HasOne(e => e.Mother)
                    .WithMany(e => e.Doughters)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Father)
                    .WithMany(e => e.Sons)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Partner)
                    .WithOne()
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);
                //entity.HasMany(e => e.Employments)
                //    .WithOne(e => e.Emploee)
                //    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                //entity.HasMany(e => e.Employed)
                //    .WithOne(e => e.Company);
            });
            modelBuilder.Entity<Employment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasOne(c => c.Emploee)
                .WithMany(c => c.Employments)
                .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(c => c.Company)
                .WithMany(c => c.Employed)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();
        }
    }
}
