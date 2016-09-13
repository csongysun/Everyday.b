using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Data
{
    public class TodoItemStore : ITodoItemStore
    {
        public ApplicationDbContext Context { get; private set; }
        private bool _disposed;

        public TodoItemStore(ApplicationDbContext context)
        {
            Context = context;
        }
        public IQueryable<TodoItem> TodoItems => Context.Set<TodoItem>();

        public async Task<TaskResult> Add(string userid, TodoItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            item.UserId = userid;
            Context.Add(item);
            await SaveChanges(cancellationToken);
            return TaskResult.Success;
        }
        private Task SaveChanges(CancellationToken cancellationToken)
        {
            return Context.SaveChangesAsync(cancellationToken);
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
            _disposed = true;
        }
    }
}
