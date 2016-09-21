using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Data
{
    public interface ITodoItemStore : IDisposable
    {
        IQueryable<TodoItem> TodoItem { get; }
        Task<TaskResult> Add(string userid, TodoItem item, CancellationToken cancellationToken);
    }

    public interface ICheckStore
    {
        IQueryable<Check> Checks { get; }
        Task<TaskResult> Check(string itemId);
        Task<TaskResult> UnCheck(string id);
    }
}