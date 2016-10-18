﻿using Everyday.b.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Identity
{
    public interface IUserStore : IDisposable
    {
        Task<TaskResult> CreateAsync(User user, CancellationToken cancellationToken);
        Task<TaskResult> UpdateAsync(User user, CancellationToken cancellationToken);
        //Task<TaskResult> FindByNameAsync(string userName, CancellationToken cancellationToken);
        Task<User> FindByIdAsync(string id, CancellationToken cancellationToken);
        Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken);
        Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken);
    }
}
