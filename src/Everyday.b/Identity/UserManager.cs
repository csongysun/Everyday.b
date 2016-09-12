using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Everyday.b.Identity
{
    public class UserManager: IDisposable
    {
        private readonly IUserStore _store;
        private readonly ITokenProvider _tokenProvider;
        private readonly IPasswordHasher _passwordHasher;
        private bool _disposed;

        public UserManager(IUserStore store,
             ITokenProvider tokenProvider,
             IPasswordHasher passwordHasher)
        {
            _store = store;
            _tokenProvider = tokenProvider;
            _passwordHasher = passwordHasher;
        }
        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        public async Task<TaskResult> CreateAsync(User user)
        {
            var result = await ValidateUser(user);

            if (!result.Succeeded)
                return result;

            user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);
            return await _store.CreateAsync(user, CancellationToken);
        }

        private static void UpdateSecurityStamp(User user)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
        }
        public async Task<TaskResult> UpdateSecurityStampAsync(User user)
        {
            UpdateSecurityStamp(user);
            return await UpdateUserAsync(user);
        }

        public async Task<TaskResult> UpdateUserAsync(User user)
        {
            var result = await ValidateUser(user);
            if (!result.Succeeded)
                return result;
            return await _store.UpdateAsync(user, CancellationToken);
        }

        public async Task<TaskResult> PasswordSignInAsync(string key,string password, string authenticationMethod = null)
        {
            var user = await FindByEmailAsync(key);
            if (CheckPassword(user, password))
            {
                var result = await SignInAsync(user);
                return !result.Succeeded ? TaskResult.Failed(result.Errors) : SignInResult.Success(user);
            }

            return SignInResult.PwdNotCorrect;
        }
        public virtual bool CheckPassword(User user, string password)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(user.PasswordHash, password);

            if (result != PasswordVerificationResult.Success)
            {
                //Logger.LogWarning(0, "Invalid password for user {userId}.", await GetUserIdAsync(user));
                return false;
            }
            return true;
        }
        public async Task<TaskResult> SignInAsync(User user, string authenticationMethod = null)
        {
            UpdateSecurityStamp(user);
            UpdateToken(user);
            return await UpdateUserAsync(user);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _store.FindByEmailAsync(email, CancellationToken);
        }

        private async Task<TaskResult> ValidateUser(User user)
        {
            //todo:添加注册校验
            return TaskResult.Success;
        }

        private void UpdateToken(User user)
        {
            DateTime expireTime;
            user.Token = _tokenProvider.GenerateToken(user, out expireTime);
            user.TokenExpires = expireTime;
        }
        public async Task<TaskResult> UpdateTokenAsync(User user)
        {
            UpdateToken(user);
            return await UpdateUserAsync(user);
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed) return;
            _store.Dispose();
            _disposed = true;
        }

    }
}
