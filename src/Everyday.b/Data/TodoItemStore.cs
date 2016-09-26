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

        public IQueryable<TodoItem> TodoItems => Context.Set<TodoItem>();


        public async Task<TaskResult> CreateAsync(TodoItem item, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            Context.Add(item);
            await SaveChanges(cancellationToken);
            return TaskResult.Success;
        }
        public async Task<TaskResult> CreateAsync(Check check,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }
            Context.Add(check);
            await SaveChanges(cancellationToken);
            return TaskResult.Success;
        }
        public async Task<TaskResult> RemoveByIdAsync<T>(string id,
            CancellationToken cancellationToken = default(CancellationToken)) where T:Entity
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var item = Context.Set<T>().FirstOrDefault(t => t.Id.Equals(id));
            if (item == null)
            {
                return TaskResult.Failed(ErrorDescriber.EntityNotFound);
            }
            Context.Set<T>().Remove(item);
            await SaveChanges(cancellationToken);
            return TaskResult.Success;
        }

        public async Task<TaskResult> UpdateAsync(TodoItem item,
            CancellationToken cancellationToken = default(CancellationToken))
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
        public async Task<TaskResult> UpdateAsync(Check check, 
            CancellationToken cancellationToken = default(CancellationToken))
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



        public Task<T> FindById<T>(string id,
            CancellationToken cancellationToken = default(CancellationToken)) where T:Entity
        {
            return Context.Set<T>().FirstOrDefaultAsync(t => t.Id.Equals(id), cancellationToken);
        }
        public Task<bool> ContainsById<T>(string id, 
            CancellationToken cancellationToken = default(CancellationToken)) where T : Entity
        {
            return Context.Set<T>().AnyAsync(t => t.Id.Equals(id), cancellationToken);
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

        public IQueryable<Check> Checks => Context.Set<Check>();

    }
}
