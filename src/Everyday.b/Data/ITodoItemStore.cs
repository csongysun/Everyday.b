using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Data
{

    public interface ITodoItemStore : IDisposable, IEntityStore
    {
        IQueryable<TodoItem> TodoItems { get; }
        Task<TaskResult> CreateAsync(TodoItem item, CancellationToken cancellationToken);
        Task<TaskResult> UpdateAsync(TodoItem item, CancellationToken cancellationToken);

    }

    public interface ICheckStore : IDisposable, IEntityStore
    {
        IQueryable<Check> Checks { get; }
        Task<TaskResult> UpdateAsync(Check check, CancellationToken cancellationToken);
        Task<TaskResult> CreateAsync(Check check, CancellationToken cancellationToken);


    }

    public interface IEntityStore
    {
        Task<T> FindById<T>(string itemId, CancellationToken cancellationToken) where T : Entity;
        Task<TaskResult> RemoveByIdAsync<T>(string id,
            CancellationToken cancellationToken = default(CancellationToken)) where T : Entity;
        Task<bool> ContainsById<T>(string id, CancellationToken cancellationToken) where T : Entity;
    }
}