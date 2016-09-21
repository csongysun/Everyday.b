using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Data
{
    public class TodoItemStore : ITodoItemStore, ICheckStore
    {
        public ApplicationDbContext Context { get; private set; }
        private bool _disposed;

        public TodoItemStore(ApplicationDbContext context)
        {
            Context = context;
        }

        public IQueryable<TodoItem> TodoItem => Context.Set<TodoItem>();


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

        //public async Task<TaskResult> FindByDate()

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

        public IQueryable<Check> Checks => Context.Set<Check>();
        public Task<TaskResult> Check(string itemId)
        {
            
            throw new NotImplementedException();
        }

        public Task<TaskResult> UnCheck(string id)
        {
            throw new NotImplementedException();
        }
    }
}
