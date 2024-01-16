using eShopping.Identity.Entities;
using eShopping.Identity.Repositories;
using Microsoft.AspNetCore.Identity;

namespace eShopping.Identity.Stores
{
    public class UserStore :
        IUserStore<User>,
        IUserPasswordStore<User>,
        IUserPhoneNumberStore<User>
    {
        private readonly IUserRepository _userRepository;

        public UserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var result = await ProcessAsync(async () =>
            {
                await _userRepository.CreateAsync(user);
            });
            return result;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            var result = await ProcessAsync(async () =>
            {
                await _userRepository.DeleteAsync(user);
            });
            return result;
        }

        public void Dispose()
        {
        }

        public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(userId, out var guid))
            {
                return null;
            }

            var user = await _userRepository.GetUserByIdAsync(guid);
            return user;
        }

        public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(normalizedUserName);
            return user;
        }

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            var result = GetUserProperty(user, user => user.PhoneNumber, cancellationToken);
            return Task.FromResult(result);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            var result = GetUserProperty(user, user => user.Id.ToString(), cancellationToken);
            return Task.FromResult(result);
        }

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            var result = GetUserProperty(user, user => user.PhoneNumber, cancellationToken);
            return Task.FromResult(result);
        }

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            if (normalizedName == null)
            {
                throw new ArgumentNullException(nameof(normalizedName));
            }

            SetUserProperty(user, normalizedName, (user, value) => user.PhoneNumber = value, cancellationToken);
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            SetUserProperty(user, userName, (user, value) => user.PhoneNumber = value, cancellationToken);
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var result = await ProcessAsync(async () =>
            {
                await _userRepository.UpdateUserAsync(user);
            });
            return result;
        }

        public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
        {
            SetUserProperty(user, passwordHash, (user, value) => user.PasswordHash = passwordHash, cancellationToken);
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            var result = GetUserProperty(user, user => user.PasswordHash, cancellationToken);
            return Task.FromResult(result);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            var passwordHash = GetUserProperty(user, user => user.PasswordHash, cancellationToken);
            var result = passwordHash != null;
            return Task.FromResult(result);
        }

        public Task SetPhoneNumberAsync(User user, string? phoneNumber, CancellationToken cancellationToken)
        {
            SetUserProperty(user, phoneNumber, (user, value) => user.PhoneNumber = phoneNumber, cancellationToken);
            return Task.CompletedTask;
        }

        public Task<string?> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            var phoneNumber = GetUserProperty(user, user => user.PhoneNumber, cancellationToken);
            return Task.FromResult(phoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            var phoneNumberConfirmed = GetUserProperty(user, user => user.PhoneNumberConfirmed, cancellationToken);
            return Task.FromResult(phoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            SetUserProperty(user, confirmed, (user, value) => user.PhoneNumberConfirmed = confirmed, cancellationToken);
            return Task.CompletedTask;
        }

        private async Task<IdentityResult> ProcessAsync(Func<Task> action)
        {
            var result = IdentityResult.Success;
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                result = IdentityResult.Failed(new IdentityError
                {
                    Description = ex.Message,
                });
            }

            return result;
        }

        private T GetUserProperty<T>(User user, Func<User, T> accessor, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return accessor(user);
        }

        private void SetUserProperty<T>(User user, T value, Action<User, T> setter, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            setter(user, value);
        }
    }
}
