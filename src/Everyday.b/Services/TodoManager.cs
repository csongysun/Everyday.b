using System.Threading;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Data;
using Everyday.b.Models;

namespace Everyday.b.Services
{
    public class TodoManager
    {
        private readonly ITodoItemStore _store;

        public TodoManager(ITodoItemStore store)
        {
            _store = store;
        }

        private CancellationToken CancellationToken =>  CancellationToken.None;

        public async Task<TaskResult> AddItemAsync(string id, TodoItem item)
        {

            return await _store.Add(id, item, CancellationToken);
        }

    }
}