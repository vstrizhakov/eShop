﻿namespace eShopping.Distribution.Entities
{
    public class UserCurrencyRate : ICurrencyRate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Rate { get; set; }
        public EmbeddedCurrency SourceCurrency { get; set; }
        public EmbeddedCurrency TargetCurrency { get; set; }
    }
}
