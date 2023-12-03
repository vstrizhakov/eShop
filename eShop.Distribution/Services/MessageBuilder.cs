using eShop.Distribution.Entities.History;
using eShop.Messaging.Contracts;
using System.Text;

namespace eShop.Distribution.Services
{
    public class MessageBuilder : IMessageBuilder
    {
        public Message FromComposition(Announce composition, DistributionSettingsRecord distributionSettings, ITextFormatter formatter)
        {
            var image = composition.Images.FirstOrDefault();

            var caption = string.Join("\n\n", composition.Products.Select(product =>
            {
                var productCaption = new StringBuilder();

                (var price, var discountedPrice, var currencyName) = CalculatePrices(product, distributionSettings);
                var showSales = distributionSettings.ShowSales;

                if (showSales && discountedPrice.HasValue)
                {
                    productCaption.Append($"{discountedPrice} {currencyName} ");
                }

                var priceLine = $"{price} {currencyName}";
                if (showSales && discountedPrice.HasValue)
                {
                    priceLine = formatter.Strikethrough(priceLine);
                }

                productCaption.Append(priceLine);

                productCaption.AppendLine();

                productCaption.AppendLine(formatter.Link(product.Name, product.Url.OriginalString));

                var description = product.Description;
                if (description != null)
                {
                    productCaption.AppendLine(description);
                }

                return productCaption.ToString();
            }));

            var message = new Message
            {
                Image = image,
                Caption = caption,
            };
            return message;
        }

        private (double price, double? discountedPrice, string currencyName) CalculatePrices(Product product, DistributionSettingsRecord distributionSettings)
        {
            var preferredCurrency = distributionSettings.PreferredCurrency;
            var currencyRates = distributionSettings.CurrencyRates;
            var comission = distributionSettings.ComissionSettings.Amount;

            var price = product.Price;
            var comissionCoefficient = (1 + comission / 100);
            var priceValue = price.Price * comissionCoefficient;
            var discountedPriceValue = price.DiscountedPrice;

            if (discountedPriceValue.HasValue)
            {
                discountedPriceValue *= comissionCoefficient;
            }

            var priceCurrency = price.Currency;
            var currencyName = priceCurrency.Name;

            if (preferredCurrency != null && preferredCurrency.Id != priceCurrency.Id)
            {
                var currencyRate = currencyRates.FirstOrDefault(e => e.CurrencyId == priceCurrency.Id);
                priceValue *= currencyRate.Rate;

                if (discountedPriceValue.HasValue)
                {
                    discountedPriceValue *= currencyRate.Rate;
                }

                currencyName = preferredCurrency.Name;
            }

            priceValue = Math.Ceiling(priceValue);
            if (discountedPriceValue.HasValue)
            {
                discountedPriceValue = Math.Ceiling(discountedPriceValue.Value);
            }

            return (priceValue, discountedPriceValue, currencyName);
        }
    }
}
