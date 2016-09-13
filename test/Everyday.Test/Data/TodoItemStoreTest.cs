using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Data;
using Everyday.b.Identity;
using Everyday.b.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit.Abstractions;

namespace Everyday.Test.Data
{
    public class TodoItemStoreTest
    {
        private readonly ITestOutputHelper output;

        public TodoItemStoreTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        [Fact]
        public async Task AddItem()
        {
            var options = CreateNewContextOptions();
            var item = new TodoItem
            {
                Title = "t0001"
            };
            var user = new User
            {
                UserName = "u0001",
                Email = "email"
            };

            using (var context = new ApplicationDbContext(options))
            {

                var ts = new TodoItemStore(context);
                var us = new UserStore(context);
                await us.CreateAsync(user);
                await ts.Add(user.Id, item);

                var users = context.Users;

                var result = users.FirstOrDefault(u => u.Id == user.Id);

               Assert.Null(result.TodoItems);
            }

        }
    }
}