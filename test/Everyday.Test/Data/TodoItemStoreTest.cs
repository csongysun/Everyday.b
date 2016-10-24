using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Data;
using Everyday.b.Identity;
using Everyday.b.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlite("Filename=test.db");

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
                Nickname = "u0001",
                Email = "email"
            };

            using (var context = new ApplicationDbContext(options))
            {
                

                var ts = new TodoItemStore(context);
                var us = new UserStore(context);
                await us.CreateAsync(user);
                await ts.CreateAsync(item);

                var users = context.Users;

                var result = users.FirstOrDefault(u => u.Id == user.Id);

               Assert.Null(result.TodoItems);
            }

        }

        [Fact]
        public async Task UpdateItem()
        {
            var options = CreateNewContextOptions();
            var item = new TodoItem
            {
                Title = "t0001",
                UserId = "00000"
            };
            var item2 = new TodoItem
            {
                Id = item.Id,
                Title = "t0001",
                UserId = "00001"
            };

            using (var context = new ApplicationDbContext(options))
            {
                var serviceProvider = context.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(new MyLoggerProvider());
                loggerFactory.AddDebug(LogLevel.Trace);

                var ts = new TodoItemStore(context);

                await context.AddAsync(item);
                context.SaveChanges();

                context.SaveChanges();

                var users = context.Users;

            }

        }
    }


    public class MyLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose()
        { }

        private class MyLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var log = formatter(state, exception);
                //Trace.WriteLine(log);
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}