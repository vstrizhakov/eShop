using Microsoft.EntityFrameworkCore;
using eShopping.Catalog.Entities;
using eShopping.Database.Extensions;
using eShopping.Database;
using eShopping.Catalog.DbContexts;

namespace eShopping.Catalog.Seedings
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
