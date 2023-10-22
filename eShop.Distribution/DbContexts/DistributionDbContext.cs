using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.DbContexts
{
    public class DistributionDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TelegramChat> TelegramChats { get; set; }
        public DbSet<ViberChat> ViberChats { get; set; }
        public DbSet<DistributionGroup> DistributionGroups { get; set; }
        public DbSet<DistributionGroupItem> DistributionGroupItems { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DistributionSettings> DistributionSettings { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<ComissionSettings> ComissionSettings { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public DistributionDbContext(DbContextOptions<DistributionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyRate>()
                .HasOne(e => e.SourceCurrency)
                .WithMany()
                .HasForeignKey(e => e.SourceCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CurrencyRate>()
                .HasOne(e => e.TargetCurrency)
                .WithMany()
                .HasForeignKey(e => e.TargetCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CurrencyRate>()
                .HasOne(e => e.DistributionSettings)
                .WithMany(e => e.CurrencyRates)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CurrencyRate>()
                .HasIndex(e => new { e.DistributionSettingsId, e.TargetCurrencyId, e.SourceCurrencyId })
                .IsUnique()
                .HasFilter(null);

            modelBuilder.Entity<DistributionSettingsHistoryRecord>()
                .HasOne(e => e.PreferredCurrency)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CurrencyRateHistoryRecord>()
                .HasOne(e => e.Currency)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CurrencyRateHistoryRecord>()
                .HasOne(e => e.DistributionSettings)
                .WithMany(e => e.CurrencyRates)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComissionSettings>()
                .HasOne(e => e.DistributionSettings)
                .WithOne(e => e.ComissionSettings)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComissionSettings>()
                .HasIndex(e => e.DistributionSettingsId)
                .IsUnique()
                .HasFilter(null);

            modelBuilder.Entity<ShopSettings>()
                .HasOne(e => e.DistributionSettings)
                .WithOne(e => e.ShopSettings)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShopSettings>()
                .HasIndex(e => e.DistributionSettingsId)
                .IsUnique()
                .HasFilter(null);

            modelBuilder.Entity<ShopSettings>()
                .HasMany(e => e.PreferredShops)
                .WithMany();

            modelBuilder.Entity<Currency>()
                .HasData(
                    new Currency
                    {
                        Id = Guid.Parse("9724739E-E4B8-45EB-AC11-EFE2B0558A34"),
                        Name = "UAH",
                    },
                    new Currency
                    {
                        Id = Guid.Parse("BF879FB6-7B4B-41C7-9CC5-DF8724D511E5"),
                        Name = "USD",
                    },
                    new Currency
                    {
                        Id = Guid.Parse("41ED0945-7196-4EAD-8F5E-DB262E62E536"),
                        Name = "EUR",
                    }
                );

            modelBuilder.Entity<CurrencyRate>()
                .HasData(new[]
                {
                    new CurrencyRate
                    {
                        Id = Guid.Parse("A3B279EA-2A65-4996-B0B5-EC242C90EBB2"),
                        TargetCurrencyId = Guid.Parse("9724739E-E4B8-45EB-AC11-EFE2B0558A34"), // UAH
                        SourceCurrencyId = Guid.Parse("BF879FB6-7B4B-41C7-9CC5-DF8724D511E5"), // USD
                        Rate = 37.09,
                        CreatedAt = DateTimeOffset.MinValue,
                    },
                    new CurrencyRate
                    {
                        Id = Guid.Parse("A05F6D6B-CCB2-43FD-8694-ECEAFDF0AB31"),
                        TargetCurrencyId = Guid.Parse("9724739E-E4B8-45EB-AC11-EFE2B0558A34"), // UAH
                        SourceCurrencyId = Guid.Parse("41ED0945-7196-4EAD-8F5E-DB262E62E536"), // EUR
                        Rate = 39.11,
                        CreatedAt = DateTimeOffset.MinValue,
                    },

                    new CurrencyRate
                    {
                        Id = Guid.Parse("D5D120A1-DBA7-4E87-A4E1-9670A365DD2D"),
                        TargetCurrencyId = Guid.Parse("BF879FB6-7B4B-41C7-9CC5-DF8724D511E5"), // USD
                        SourceCurrencyId = Guid.Parse("9724739E-E4B8-45EB-AC11-EFE2B0558A34"), // UAH
                        Rate = 0.027,
                        CreatedAt = DateTimeOffset.MinValue,
                    },
                    new CurrencyRate
                    {
                        Id = Guid.Parse("6FC5E363-A92E-4412-9DBB-C84841E06F91"),
                        TargetCurrencyId = Guid.Parse("BF879FB6-7B4B-41C7-9CC5-DF8724D511E5"), // USD
                        SourceCurrencyId = Guid.Parse("41ED0945-7196-4EAD-8F5E-DB262E62E536"), // EUR
                        Rate = 1.05,
                        CreatedAt = DateTimeOffset.MinValue,
                    },

                    new CurrencyRate
                    {
                        Id = Guid.Parse("E4021EE6-1568-448C-A95B-AC76191F235B"),
                        TargetCurrencyId = Guid.Parse("41ED0945-7196-4EAD-8F5E-DB262E62E536"), // EUR
                        SourceCurrencyId = Guid.Parse("9724739E-E4B8-45EB-AC11-EFE2B0558A34"), // UAH
                        Rate = 0.026,
                        CreatedAt = DateTimeOffset.MinValue,
                    },
                    new CurrencyRate
                    {
                        Id = Guid.Parse("69D06081-F197-4823-8248-EB6C60CB73A4"),
                        TargetCurrencyId = Guid.Parse("41ED0945-7196-4EAD-8F5E-DB262E62E536"), // EUR
                        SourceCurrencyId = Guid.Parse("BF879FB6-7B4B-41C7-9CC5-DF8724D511E5"), // USD
                        Rate = 0.95,
                        CreatedAt = DateTimeOffset.MinValue,
                    },
                });
        }
    }
}
