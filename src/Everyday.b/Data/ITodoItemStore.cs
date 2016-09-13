using System;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Data
{
    public interface ITodoItemStore : IDisposable
    {
        Task<TaskResult> Add(string userid, TodoItem item, CancellationToken cancellationToken);
    }
}