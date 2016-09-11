using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;
using Microsoft.AspNetCore.Http;

namespace Everyday.b.Identity
{
    public class UserManager
    {
        private readonly IUserStore Store;
        private readonly ITokenProvider _tokenProvider;
        public UserManager(IUserStore store, ITokenProvider tokenProvider)
        {
            Store = store;
            _tokenProvider = tokenProvider;
        }
        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        public async Task<TaskResult> CreateAsync(User user)
        {
            var result = await ValidateUser(user);
            if (!result.Succeeded)
                return result;

            return await Store.CreateAsync(user, CancellationToken);
        }

        public async Task<TaskResult> UpdateSecurityStampAsync(User user)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            return await UpdateUserAsync(user);
        }

        public async Task<TaskResult> UpdateUserAsync(User user)
        {
            var result = await ValidateUser(user);
            if (!result.Succeeded)
                return result;
            return await Store.UpdateAsync(user, CancellationToken);
        }

        public async Task<TaskResult> SignInAsync(User user, string authenticationMethod = null)
        {
            await UpdateSecurityStampAsync(user);
            return TaskResult.Success;
        }

        private async Task<TaskResult> ValidateUser(User user)
        {

            return TaskResult.Success;
        }

        public async Task<TaskResult> UpdateToken(User user)
        {
            DateTime expireTime;
            user.Token = _tokenProvider.GenerateToken(user,out expireTime);
            user.TokenExpires = expireTime;
            return await UpdateUserAsync(user);
        }

    }
}
