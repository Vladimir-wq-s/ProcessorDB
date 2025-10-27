using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProcessorDB.Models;
using System;
using System.Collections.Generic;

namespace ProcessorDB.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Processor> Processors { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<TechSpec> TechSpecs { get; set; }
        public DbSet<ProductionInfo> ProductionInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=processors.db");
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Processor>()
                .HasOne(p => p.TechSpec)
                .WithOne()
                .HasForeignKey<TechSpec>(t => t.ProcessorId);

            modelBuilder.Entity<Processor>()
                .HasOne(p => p.ProductionInfo)
                .WithOne()
                .HasForeignKey<ProductionInfo>(p => p.ProcessorId);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Статические реальные данные
            var manufacturers = new List<Manufacturer>
            {
                new Manufacturer { Id = 1, Name = "AMD" },
                new Manufacturer { Id = 2, Name = "Intel" }
            };
            modelBuilder.Entity<Manufacturer>().HasData(manufacturers);

            var countries = new List<Country>
            {
                new Country { Id = 1, Name = "USA" },
                new Country { Id = 2, Name = "Taiwan" }
            };
            modelBuilder.Entity<Country>().HasData(countries);

            var processors = new List<Processor>
            {
                new Processor { Id = 1, Name = "Ryzen 5 8400F", Model = "Ryzen 5 8400F", ManufacturerId = 1, CountryId = 2, ReleaseYear = 2024 },
                new Processor { Id = 2, Name = "Ryzen 7 7800X3D", Model = "Ryzen 7 7800X3D", ManufacturerId = 1, CountryId = 2, ReleaseYear = 2023 },
                new Processor { Id = 3, Name = "Core i5-12400F", Model = "Core i5-12400F", ManufacturerId = 2, CountryId = 1, ReleaseYear = 2022 },
                new Processor { Id = 4, Name = "Core i7-13700K", Model = "Core i7-13700K", ManufacturerId = 2, CountryId = 1, ReleaseYear = 2023 }
            };
            var techSpecs = new List<TechSpec>
            {
                new TechSpec { Id = 1, ProcessorId = 1, TechProcess = "4nm", Frequency = 4.7, CacheL3 = "16MB", Cores = 6, Slot = "AM5" },
                new TechSpec { Id = 2, ProcessorId = 2, TechProcess = "5nm", Frequency = 4.2, CacheL3 = "96MB", Cores = 8, Slot = "AM5" },
                new TechSpec { Id = 3, ProcessorId = 3, TechProcess = "10nm", Frequency = 2.5, CacheL3 = "18MB", Cores = 6, Slot = "LGA 1700" },
                new TechSpec { Id = 4, ProcessorId = 4, TechProcess = "10nm", Frequency = 3.4, CacheL3 = "30MB", Cores = 16, Slot = "LGA 1700" }
            };
            var productionInfos = new List<ProductionInfo>
            {
                new ProductionInfo { Id = 1, ProcessorId = 1, ProductionDate = new DateTime(2024, 1, 1), WarrantyPeriod = 36, Price = 199, Points = 1200, Promotion = false },
                new ProductionInfo { Id = 2, ProcessorId = 2, ProductionDate = new DateTime(2023, 9, 1), WarrantyPeriod = 36, Price = 449, Points = 1800, Promotion = true },
                new ProductionInfo { Id = 3, ProcessorId = 3, ProductionDate = new DateTime(2022, 1, 1), WarrantyPeriod = 36, Price = 179, Points = 1100, Promotion = false },
                new ProductionInfo { Id = 4, ProcessorId = 4, ProductionDate = new DateTime(2023, 9, 1), WarrantyPeriod = 36, Price = 409, Points = 1600, Promotion = true }
            };
            modelBuilder.Entity<Processor>().HasData(processors);
            modelBuilder.Entity<TechSpec>().HasData(techSpecs);
            modelBuilder.Entity<ProductionInfo>().HasData(productionInfos);
        }
    }
}
