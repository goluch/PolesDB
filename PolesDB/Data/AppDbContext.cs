﻿using DataBase.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
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
            //Comment this during updatnig db usung ef tools!!!
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
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.OwnsOne(e => e.Gender, navigationBuilder =>
                {
                    navigationBuilder.Property(p => p.Value)
                                     .HasColumnName("Gender");
                });
                entity.HasOne(e => e.Parent)
                    .WithMany(e => e.Children);
                entity.HasOne(e => e.Partner)
                    .WithOne()
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.Employments)
                    .WithOne(e => e.Emploee)
                    .OnDelete(DeleteBehavior.NoAction);

            });
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasMany(e => e.Employed)
                    .WithOne(e => e.Company);
            });
            modelBuilder.Entity<Employment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
