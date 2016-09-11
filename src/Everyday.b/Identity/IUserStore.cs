using Everyday.b.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Identity
{
    public interface IUserStore
    {
        Task<TaskResult> CreateAsync(User user, CancellationToken cancellationToken);
        Task<TaskResult> UpdateAsync(User user, CancellationToken cancellationToken);

    }
}
