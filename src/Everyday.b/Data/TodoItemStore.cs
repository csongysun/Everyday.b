using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<TaskResult> UpdateAsync(TodoItem item, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Context.Attach(item);
            Context.Update(item);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return TaskResult.Failed(ErrorDescriber.ConcurrencyFailure);
            }
            return TaskResult.Success;
        }
        public async Task<TaskResult> UpdateAsync(Check check, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            Context.Attach(check);
            Context.Update(check);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return TaskResult.Failed(ErrorDescriber.ConcurrencyFailure);
            }
            return TaskResult.Success;
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

        public IQueryable<Check> Checks => Context.Set<Check>();

    }
}
