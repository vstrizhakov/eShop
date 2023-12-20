using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.DbContexts
{
    public class DistributionDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DefaultCurrencyRate> DefaultCurrencyRates { get; set; }
        public DbSet<Entities.Distribution> Distribution { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public DistributionDbContext(DbContextOptions<DistributionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAutoscaleThroughput(1000);

            modelBuilder.Entity<Account>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<Currency>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<Entities.Distribution>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<DefaultCurrencyRate>()
                .HasPartitionKey(e => e.PartitionKey);

            modelBuilder.Entity<Shop>()
                .HasPartitionKey(e => e.PartitionKey);

            var uah = new Currency
            {
                Id = Guid.Parse("9724739E-E4B8-45EB-AC11-EFE2B0558A34"),
                Name = "UAH",
            };
            var usd = new Currency
            {
                Id = Guid.Parse("BF879FB6-7B4B-41C7-9CC5-DF8724D511E5"),
                Name = "USD",
            };
            var eur = new Currency
            {
                Id = Guid.Parse("41ED0945-7196-4EAD-8F5E-DB262E62E536"),
                Name = "EUR",
            };

            modelBuilder.Entity<Currency>()
                .HasData(uah, usd, eur);

            //modelBuilder.Entity<DefaultCurrencyRate>()
            //    .HasData(new[]
            //    {
            //        new DefaultCurrencyRate
            //        {
            //            Id = Guid.Parse("A3B279EA-2A65-4996-B0B5-EC242C90EBB2"),
            //            TargetCurrency = uah.GeneratedEmbedded(), // UAH
            //            SourceCurrency = usd.GeneratedEmbedded(), // USD
            //            Rate = 37.09,
            //        },
            //        new DefaultCurrencyRate
            //        {
            //            Id = Guid.Parse("A05F6D6B-CCB2-43FD-8694-ECEAFDF0AB31"),
            //            TargetCurrency = uah.GeneratedEmbedded(), // UAH
            //            SourceCurrency = eur.GeneratedEmbedded(), // EUR
            //            Rate = 39.11,
            //            CreatedAt = DateTimeOffset.MinValue,
            //        },

            //        new DefaultCurrencyRate
            //        {
            //            Id = Guid.Parse("D5D120A1-DBA7-4E87-A4E1-9670A365DD2D"),
            //            TargetCurrency = usd.GeneratedEmbedded(), // USD
            //            SourceCurrency = uah.GeneratedEmbedded(), // UAH
            //            Rate = 0.027,
            //            CreatedAt = DateTimeOffset.MinValue,
            //        },
            //        new DefaultCurrencyRate
            //        {
            //            Id = Guid.Parse("6FC5E363-A92E-4412-9DBB-C84841E06F91"),
            //            TargetCurrency = usd.GeneratedEmbedded(), // USD
            //            SourceCurrency = eur.GeneratedEmbedded(), // EUR
            //            Rate = 1.05,
            //            CreatedAt = DateTimeOffset.MinValue,
            //        },

            //        new DefaultCurrencyRate
            //        {
            //            Id = Guid.Parse("E4021EE6-1568-448C-A95B-AC76191F235B"),
            //            TargetCurrency = eur.GeneratedEmbedded(), // EUR
            //            SourceCurrency = uah.GeneratedEmbedded(), // UAH
            //            Rate = 0.026,
            //            CreatedAt = DateTimeOffset.MinValue,
            //        },
            //        new DefaultCurrencyRate
            //        {
            //            Id = Guid.Parse("69D06081-F197-4823-8248-EB6C60CB73A4"),
            //            TargetCurrency = eur.GeneratedEmbedded(), // EUR
            //            SourceCurrency = usd.GeneratedEmbedded(), // USD
            //            Rate = 0.95,
            //            CreatedAt = DateTimeOffset.MinValue,
            //        },
            //    });
        }
    }
}
