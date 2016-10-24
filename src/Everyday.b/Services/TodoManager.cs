using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Data;
using Everyday.b.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        public async Task<TaskResult> DeleteItemAsync(string itemId, string userId)
        {
            return await _store.DeleteAsync(itemId, userId, CancellationToken);
        }
        public async Task<TaskResult> UpdateItemAsync(TodoItemModel model, string userId)
        {
            //var item = new TodoItem
            //{
            //    Id = model.Id,
            //    Title = model.Title,
            //    BeginDate = model.BeginDate,
            //    EndDate = model.EndDate,
            //    UserId = userId
            //};
            //return await _store.UpdateAsync(item, userId, CancellationToken);
            var item = new TodoItem
            {
                Id = model.Id,
                Title = model.Title,
                BeginDate = model.BeginDate,
                EndDate = model.EndDate,
                UserId = userId
            };
            if (await _store.TodoItems.AnyAsync(t => t.Id == model.Id && t.UserId == userId, CancellationToken))
            {
                return await _store.UpdateAsync(item, CancellationToken);
            }
            return EntityResult.EntityNotFound;
        }

        public async Task<IList<TodoItem>> GetTodayItems(string id, DateTime date)
        {
            string datesql = $"make_date({date.Year},{date.Month},{date.Day})";

            return
                await
                    _store.TodoItems.FromSql($"SELECT * FROM [TodoItems] AS t WHERE t.UserId = '{id}' AND t.BeginDate <= {datesql} AND t.EndDate >= {datesql}")
                    //.Where(t => t.UserId == id && t.BeginDate <= date.Date && t.EndDate >= date.Date)
                        .ToListAsync(CancellationToken);
        }

        public async Task<IList<TodoItem>> GetAllItems(string id)
        {
            return await _store.TodoItems.Where(t => t.UserId == id)
                .Include(t => t.Checks)
                .ToListAsync(CancellationToken);
        }

        public async Task<TaskResult> Check(string itemId, string userId)
        {
            if (!await _store.PermissionCheckAsync(itemId, userId, CancellationToken))
            {
                return EntityResult.EntityNotFound;
            }
            var checkStore = GetCheckStore();
            return await checkStore.CheckAsync(itemId, CancellationToken);

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