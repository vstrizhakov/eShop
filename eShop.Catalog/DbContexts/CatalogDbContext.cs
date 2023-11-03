using eShop.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.DbContexts
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<Announce> Announces { get; set; }
        public DbSet<AnnounceImage> AnnounceImages { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOne(e => e.ParentCategory)
                .WithMany(e => e.SubCategories)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shop>()
                .HasData(
                    new Shop
                    {
                        Id = Guid.Parse("7EBDC9B0-4846-415F-AF63-29D74E4B7B36"),
                        Name = "Nike",
                        CreatedAt = DateTimeOffset.MinValue,
                    },
                    new Shop
                    {
                        Id = Guid.Parse("17A27EC2-EB71-4DE8-BE81-734AEFFD28F9"),
                        Name = "Puma",
                        CreatedAt = DateTimeOffset.MinValue,
                    },
                    new Shop
                    {
                        Id = Guid.Parse("9AFC47E2-3866-42F8-96DB-1042800AAFA7"),
                        Name = "Adidas",
                        CreatedAt = DateTimeOffset.MinValue,
                    }
                );

            modelBuilder.Entity<Currency>()
                .HasData(
                    new Currency
                    {
                        Id = Guid.Parse("9724739E-E4B8-45EB-AC11-EFE2B0558A34"),
                        Name = "UAH",
                        CreatedAt = DateTimeOffset.MinValue,
                    },
                    new Currency
                    {
                        Id = Guid.Parse("BF879FB6-7B4B-41C7-9CC5-DF8724D511E5"),
                        Name = "USD",
                        CreatedAt = DateTimeOffset.MinValue,
                    },
                    new Currency
                    {
                        Id = Guid.Parse("41ED0945-7196-4EAD-8F5E-DB262E62E536"),
                        Name = "EUR",
                        CreatedAt = DateTimeOffset.MinValue,
                    }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
