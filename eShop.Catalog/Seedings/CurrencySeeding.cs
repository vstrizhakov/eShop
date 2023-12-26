using eShop.Catalog.DbContexts;
using eShop.Database;
using eShop.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using eShop.Catalog.Entities;

namespace eShop.Catalog.Seedings
{
    public class CurrencySeeding : IDataSeeding<CatalogDbContext>
    {
        public async Task SeedAsync(CatalogDbContext context)
        {
            var currencies = await context.Currencies
                .WithDiscriminatorAsPartitionKey()
                .ToListAsync();

            var seed = new[]
            {
                new Currency
                {
                    Name = "UAH",
                },
                new Currency
                {
                    Name = "USD",
                },
                new Currency
                {
                    Name = "EUR",
                },
            };


            foreach (var currency in seed)
            {
                if (!currencies.Any(e => e.Name == currency.Name))
                {
                    context.Currencies.Add(currency);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
