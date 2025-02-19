using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace ASP.Seminar1.Models
{
    public class ProductContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ProductContext(DbContextOptions<ProductContext> options, IConfiguration configuration) 
        {
          _configuration = configuration;
        }

        public DbSet<Storage> Storages { get; set; }
       public DbSet<Product> Products { get; set; }
       public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(en =>
            {
                en.ToTable("Products");
                
                en.HasKey(x => x.Id).HasName("ProductID");
                en.HasIndex(x => x.Name).IsUnique();

                en.Property(e => e.Name)
                  .HasColumnName("ProductName")
                  .HasMaxLength(255)
                  .IsRequired();

                en.Property(e => e.Description)
                  .HasColumnName("Description")
                  .HasMaxLength(255)
                  .IsRequired();

                en.Property(e => e.Cost)
                  .HasColumnName("Cost")
                  .IsRequired();

                en.HasOne(x => x.Category)
                  .WithMany(c =>c.Products)
                  .HasForeignKey(x => x.CategoryId)
                  .HasConstraintName("CategoryToProduct");   
            });

            modelBuilder.Entity<Category>(en =>
            {
                en.ToTable("Categories");

                en.HasKey(x => x.Id).HasName("CategoryID");
                en.HasIndex(x =>x.Name).IsUnique();

                en.Property(e => e.Name)
                  .HasColumnName("CategoryName")
                  .HasMaxLength(255)
                  .IsRequired();

                en.Property(e => e.Description)
                  .HasColumnName("Description")
                  .HasMaxLength(255)
                  .IsRequired();

                
            });

            modelBuilder.Entity<Storage>(en =>
            {
                en.ToTable("Storage");

                en.HasKey(x => x.Id).HasName("StorageId");

                en.Property(en => en.Name)
                  .HasColumnName("StorageName");

                en.Property(en => en.Count)
                  .HasColumnName("Count");

                en.HasMany(x => x.Products)
                   .WithMany(c => c.ProductStorages)
                   .UsingEntity(j => j.ToTable("StorageProduct"));
                 
            });
        }

    }
}
