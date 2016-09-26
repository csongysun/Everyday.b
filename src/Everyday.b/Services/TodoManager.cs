using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Data;
using Everyday.b.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Everyday.b.Services
{
    public class TodoManager
    {
        private readonly ITodoItemStore _store;

        public TodoManager(ITodoItemStore store)
        {
            _store = store;
        }

        private CancellationToken CancellationToken => CancellationToken.None;

        public async Task<TaskResult> AddItemAsync(string userId, TodoItem item)
        {
            item.Id = Guid.NewGuid().ToString();
            item.UserId = userId;
            return await _store.CreateAsync(item, CancellationToken);
        }
        public async Task<TaskResult> DeleteItemAsync(string itemId)
        {
            
            return await _store.RemoveByIdAsync<TodoItem>(itemId, CancellationToken);
        }

        public async Task<IList<TodoItem>> GetTodayItems(string id, DateTime date)
        {
            return
                await
                    _store.TodoItems.Where(t => t.UserId == id && t.BeginDate.Date <= date.Date && t.EndDate.Date >= date.Date)
                        .Include(t => t.Checks)
                        .ToListAsync(CancellationToken);
        }

        public async Task<IList<TodoItem>> GetAllItems(string id)
        {
            return await _store.TodoItems.Where(t => t.UserId == id)
                .Include(t => t.Checks)
                .ToListAsync(CancellationToken);
        }

        public async Task<TaskResult> Check(string itemId)
        {
            var checkStore = GetCheckStore();
            var nday = DateTime.Today + TimeSpan.FromDays(1);
            var exist = checkStore.Checks.Any(c => c.TodoItemId == itemId && c.CheckedDate >= DateTime.Today && c.CheckedDate <= nday);
            Check check;
            if (!exist)
            {
                if (!await _store.ContainsById<TodoItem>(itemId, CancellationToken))
                    return TodoResult.ItemNotFound;
                check = new Check
                {
                    Checked = true,
                    CheckedDate = DateTime.Today,
                    TodoItemId = itemId
                };
                return await checkStore.CreateAsync(check, CancellationToken);
            }
            check = checkStore.Checks.First(c => c.TodoItemId == itemId && c.CheckedDate >= DateTime.Today && c.CheckedDate <= nday);
            check.Checked = !check.Checked;
            return await checkStore.UpdateAsync(check, CancellationToken);
        }

        private ICheckStore GetCheckStore()
        {
            var cast = _store as ICheckStore;
            if (cast == null)
            {
                throw new NotSupportedException(Resource.StoreNotICheckStore);
            }
            return cast;
        }

        public class TodoResult : TaskResult<TodoItem>
        {
            public static TodoResult ItemNotFound
                => new TodoResult {Errors = new List<Error> {ErrorDescriber.ItemNotFound}};
            public static TodoResult CheckNotFound
                => new TodoResult { Errors = new List<Error> { ErrorDescriber.CheckNotFound } };
        }

    }
}