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

        public async Task<TaskResult> DeleteByIdAsync<T>(string id,
            CancellationToken cancellationToken = default(CancellationToken)) where T : Entity, new()
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var item = new T {Id = id};
            Context.Entry(item).State = EntityState.Deleted;
            await SaveChanges(cancellationToken);
            return TaskResult.Success;
        }

        public async Task<TaskResult> DeleteAsync(string itemId, string userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException();
            }
            using (Context.Database.BeginTransaction())
            {
                try
                {
                    var result = await
                    Context.Database.ExecuteSqlCommandAsync(
                        $"DELETE FROM TodoItems WHERE Id = '{itemId}' AND UserId = '{userId}'", cancellationToken);
                    if (result != 1) return EntityResult.EntityNotFound;
                    await Context.Database.ExecuteSqlCommandAsync(
                                $"DELETE FROM Checks WHERE TodoItemId = '{itemId}'", cancellationToken);
                }
                catch (Exception e)
                {
                    return EntityResult.SqlFailed(e.Message);
                }
            }
            return TaskResult.Success;
        }
        public async Task<TaskResult> DeleteAsync<T>(T entity,
    CancellationToken cancellationToken = default(CancellationToken)) where T:Entity
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            Context.Remove(entity);
            await SaveChanges(cancellationToken);
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

        public async Task<TaskResult> UpdateAsync(TodoItem item, string userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            int result;
            using (Context.Database.BeginTransaction())
            {
                result =
                    await
                        Context.Database.ExecuteSqlCommandAsync(
                            $"UPDATE TodoItems SET Title = '{item.Title}', BeginDate = '{item.BeginDate}', EndDate = '{item.EndDate}' WHERE Id = {item.Id} AND UserId = {userId}", cancellationToken);

            }
            return result == 1 ? TaskResult.Success : EntityResult.EntityNotFound;
        }

        public async Task<TaskResult> UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : Entity
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            //Context.Attach(entity);
            // ConcurrencyStamp update
            Context.Update(entity);
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

        public async Task<TaskResult> CheckAsync(string itemId, CancellationToken cancellationToken)
        {
            using (Context.Database.BeginTransaction())
            {
                var nday = DateTime.Today.AddDays(1);

                string today = $"make_date({DateTime.Today.Year},{DateTime.Today.Month},{DateTime.Today.Day})";
                //var check =
                //    await Context.Checks.FromSql(
                //            $"SELECT * FROM Checks AS c WHERE c.TodoItemId = '{itemId}' AND c.CheckedDate = {today}")
                //        .FirstOrDefaultAsync(cancellationToken);
                var check = await Checks.FirstOrDefaultAsync(c => c.TodoItemId == itemId && c.CheckedDate == DateTime.Today, cancellationToken);

                if (check == null)
                {
                    check = new Check
                    {
                        Checked = true,
                        CheckedDate = DateTime.Today,
                        TodoItemId = itemId
                    };
                    return await CreateAsync(check, cancellationToken);
                }
                check.Checked = !check.Checked;
                return await UpdateAsync(check, cancellationToken);
            }
            
        }

        public async Task<bool> PermissionCheckAsync(string itemId, string userId, CancellationToken cancellationToken)
        {
            using (var tt = Context.Database.BeginTransaction())
            {
                bool r = await TodoItems.AnyAsync(t => t.Id == itemId && t.UserId == userId, cancellationToken);
                
                //tt.Commit();
                return r;
            }

        }

        public async Task<IList<TodoItem>> GetItemsByDate(string userId, DateTime date,
            CancellationToken cancellationToken)
        {
            string datesql = $"make_date({date.Year},{date.Month},{date.Day})";
            return
                await
                    TodoItems.FromSql("SELECT * FROM {0} WHERE UserId = {1} ", "TodoItems", userId )
                        //.Where(t => t.UserId == id && t.BeginDate <= date.Date && t.EndDate >= date.Date)
                       // .Include(t => t.Checks)
                        .ToListAsync(cancellationToken);
        }

        public IQueryable<Check> Checks => Context.Set<Check>();

    }
}
