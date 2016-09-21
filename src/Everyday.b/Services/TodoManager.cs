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
    }
}