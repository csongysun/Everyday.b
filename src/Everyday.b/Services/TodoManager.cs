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

        public async Task<TaskResult> AddItemAsync(string id, TodoItem item)
        {

            return await _store.Add(id, item, CancellationToken);
        }

        public async Task<IList<TodoItem>> GetTodayItems(string id, DateTime date)
        {
            return
                await
                    _store.TodoItem.Where(t => t.UserId == id && t.BeginDate.Date <= date.Date && t.EndDate.Date >= date.Date)
                        .Include(t => t.Checks)
                        .ToListAsync(CancellationToken);
        }

        public async Task<IList<TodoItem>> GetAllItems(string id)
        {
            return await _store.TodoItem.Where(t => t.UserId == id)
                .Include(t => t.Checks)
                .ToListAsync(CancellationToken);
        }

        public async Task<TodoItem> FindById(string id)
        {
            return await _store.TodoItem.Include(t => t.Checks).FirstOrDefaultAsync(t => t.Id == id, CancellationToken);
        }

        public async Task<TaskResult> Check(string itemId)
        {
            var item = await FindById(itemId);
            if (item == null)
                return TodoResult.ItemNotFound;
            var check = item.Checks.FirstOrDefault(c => c.CheckedDate.Date == DateTime.Today);
            if (check == null)
            {
                item.Checks.Add(new Check
                {
                    Checked = true,
                    CheckedDate = DateTime.Today,
                    //TodoItemId = item.Id;
                });
            }
            else
            {
                check.Checked = !check.Checked;
            }
            return await _store.UpdateAsync(item, CancellationToken);
        }

        public async Task<TaskResult> UnCheck(string itemId)
        {
            //var checkStore = _store as ICheckStore;
            //Check check = null;
            //if (checkStore != null)
            //{
            //    check = checkStore.Checks.FirstOrDefault(c => c.CheckedDate.Date == DateTime.Today);
            //}

            var item = await FindById(itemId);
            if (item == null)
                return TodoResult.ItemNotFound;
            var check = item.Checks.FirstOrDefault(c => c.CheckedDate.Date == DateTime.Today);
            if (check == null) return TodoResult.CheckNotFound;
            check.Checked = false;
            return await _store.UpdateAsync(item, CancellationToken);
        }

        public class TodoResult : TaskResult<TodoItem>
        {
            public static TodoResult ItemNotFound
                => new TodoResult {Errors = new List<Error> {ErrorDescriber.ItemNotFound}};
            public static TodoResult CheckNotFound
                => new TodoResult { Errors = new List<Error> { ErrorDescriber.CheckNotFound } };
        }

        public static partial class ErrorDescriber
        {
            public static Error ItemNotFound => new Error
            {
                Code = nameof(ItemNotFound),
                Description = Resource.ItemNotFound
            };
            public static Error CheckNotFound => new Error
            {
                Code = nameof(CheckNotFound),
                Description = Resource.CheckNotFound
            };
        }
    }
}