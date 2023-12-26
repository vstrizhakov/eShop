using eShop.Distribution.Aggregates;
using eShop.Distribution.Entities;
using eShop.Distribution.Exceptions;
using eShop.Distribution.Repositories;

namespace eShop.Distribution.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IDefaultCurrencyRateRepository _defaultCurrencyRateRepository;

        public AccountService(
            IAccountRepository accountRepository,
            IShopRepository shopRepository,
            IDefaultCurrencyRateRepository defaultCurrencyRateRepository)
        {
            _accountRepository = accountRepository;
            _shopRepository = shopRepository;
            _defaultCurrencyRateRepository = defaultCurrencyRateRepository;
        }

        public async Task UpdateTelegramChatAsync(Guid accountId, Guid telegramChatId, bool isEnabled)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }

            await _accountRepository.UpdateTelegramChatAsync(account, telegramChatId, isEnabled);
        }

        public async Task UpdateViberChatAsync(Guid accountId, Guid viberChatId, bool isEnabled)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException();
            }

            await _accountRepository.UpdateViberChatAsync(account, viberChatId, isEnabled);
        }

        public async Task CreateAccountAsync(Guid accountId, Guid? telegramUserId, Guid? viberUserId, string firstName, string lastName, Guid? announcerId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                throw new AccountAlreadyExistsException();
            }

            account = new Account
            {
                Id = accountId,
                TelegramUserId = telegramUserId,
                ViberUserId = viberUserId,
                FirstName = firstName,
                LastName = lastName,
                AnnouncerId = announcerId,
                IsActivated = true,
            };

            await _accountRepository.CreateAccountAsync(account);
        }

        public async Task UpdateAccountAsync(Guid accountId, Guid? telegramUserId, Guid? viberUserId, string firstName, string lastName)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new InvalidOperationException(); // TODO: custom error
            }

            account.TelegramUserId = telegramUserId;
            account.ViberUserId = viberUserId;
            account.FirstName = firstName;
            account.LastName = lastName;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task SubscribeToAnnouncerAsync(Account account, Account announcer)
        {
            account.AnnouncerId = announcer.Id;
            account.IsActivated = true;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task<Account?> GetAccountByIdAsync(Guid accountId)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            return account;
        }

        public async Task<Account?> GetAccountByTelegramUserIdAsync(Guid accountId)
        {
            var account = await _accountRepository.GetAccountByTelegramUserIdAsync(accountId);
            return account;
        }

        public async Task<Account?> GetAccountByViberUserIdAsync(Guid accountId)
        {
            var account = await _accountRepository.GetViberByTelegramUserIdAsync(accountId);
            return account;
        }

        public async Task SetPreferredCurrencyAsync(Account account, Currency currency)
        {
            var distributionSettings = account.DistributionSettings;
            distributionSettings.PreferredCurrency = currency.GeneratedEmbedded();

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task<IEnumerable<ICurrencyRate>> GetCurrencyRatesAsync(Account account)
        {
            var distributionSettings = account.DistributionSettings;
            if (distributionSettings.PreferredCurrency == null)
            {
                throw new InvalidOperationException(); // TODO: not sure we need this a per history record
            }

            var preferredCurrencyId = distributionSettings.PreferredCurrency.Id;
            var defaultCurrencyRates = await _defaultCurrencyRateRepository.GetCurrencyRatesAsync(preferredCurrencyId);
            var customCurrencyRates = distributionSettings.CurrencyRates.Where(e => e.TargetCurrency.Id == preferredCurrencyId);

            var currencyRates = new List<ICurrencyRate>(customCurrencyRates.Count() + defaultCurrencyRates.Count());
            currencyRates.AddRange(customCurrencyRates);
            currencyRates.AddRange(defaultCurrencyRates);

            return currencyRates.DistinctBy(e => e.SourceCurrency.Id);
        }

        public async Task SetCurrencyRateAsync(Account account, Currency sourceCurrency, float rate)
        {
            var distributionSettings = account.DistributionSettings;
            if (distributionSettings.PreferredCurrency == null)
            {
                throw new InvalidOperationException(); // TODO: not sure we need this a per history record
            }

            var targetCurrency = distributionSettings.PreferredCurrency;
            var currencyRates = distributionSettings.CurrencyRates;

            var currencyRate = currencyRates.FirstOrDefault(e => e.TargetCurrency.Id == targetCurrency.Id && e.TargetCurrency.Id == sourceCurrency.Id);
            if (currencyRate == null)
            {
                var defaultCurrencyRates = await _defaultCurrencyRateRepository.GetCurrencyRatesAsync(targetCurrency.Id);
                if (!defaultCurrencyRates.Any(e => e.SourceCurrency.Id == sourceCurrency.Id))
                {
                    throw new InvalidOperationException();
                }

                currencyRate = new UserCurrencyRate
                {
                    TargetCurrency = targetCurrency,
                    SourceCurrency = sourceCurrency.GeneratedEmbedded(),
                };

                currencyRates.Add(currencyRate);
            }

            currencyRate.Rate = rate;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task SetComissionAmountAsync(Account account, double amount)
        {
            var distributionSettings = account.DistributionSettings;
            distributionSettings.ComissionSettings.Amount = amount;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task SetFilterShopsAsync(Account account, bool filter)
        {
            var distributionSettings = account.DistributionSettings;
            distributionSettings.ShopSettings.Filter = filter;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task<IEnumerable<ShopFilter>> GetShopsAsync(Account account)
        {
            var distributionSettings = account.DistributionSettings;
            if (!distributionSettings.ShopSettings.Filter)
            {
                throw new InvalidOperationException();
            }

            var shops = await _shopRepository.GetShopsAsync();

            var filterShops = distributionSettings.ShopSettings.PreferredShops
                .Select(shop => new ShopFilter(shop, true));
            filterShops = filterShops.Concat(shops.Select(shop => new ShopFilter(shop.GenerateEmbedded(), false)));
            filterShops = filterShops.DistinctBy(e => e.Shop.Id);
            filterShops = filterShops.OrderBy(e => e.Shop.Name);

            return filterShops;
        }

        public async Task SetShopIsEnabledAsync(Account account, Guid shopId, bool isEnabled)
        {
            var distributionSettings = account.DistributionSettings;
            var shops = distributionSettings.ShopSettings.PreferredShops;
            var existingShop = shops.FirstOrDefault(e => e.Id == shopId);
            if (isEnabled)
            {
                if (existingShop == null)
                {
                    var shop = await _shopRepository.GetShopAsync(shopId);
                    shops.Add(shop.GenerateEmbedded());
                }
            }
            else
            {
                shops.Remove(existingShop);
            }

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task SetShowSalesAsync(Account account, bool showSales)
        {
            var distributionSettings = account.DistributionSettings;
            distributionSettings.ShowSales = showSales;

            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task<IEnumerable<Account>> GetSubscribersAsync(Guid announcerId)
        {
            var subscribers = await _accountRepository.GetAccountsByAnnouncerIdAsync(announcerId);
            return subscribers;
        }
    }
}
