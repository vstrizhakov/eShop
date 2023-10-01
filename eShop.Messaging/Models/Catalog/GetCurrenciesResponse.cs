namespace eShop.Messaging.Models.Catalog
{
    public record GetCurrenciesResponse(Guid AccountId, IEnumerable<Currency> Currencies);
}
