using System;
using System.Diagnostics;
using Everyday.b.Common;
using Everyday.b.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Everyday.b.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Check> Checks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Email).HasName("EmailIndex").IsUnique();
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.Nickname).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);

                b.HasMany(u => u.TodoItems)
                    .WithOne(t=>t.User)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(t=>t.UserId)
                    .IsRequired();
                
            });

            builder.Entity<TodoItem>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasIndex(t => t.EndDate).HasName("EndDateIndex");
                b.HasIndex(t => t.BeginDate).HasName("BeginDateIndex");
                b.Property(t => t.Title).HasMaxLength(256);
               //b.Property(t => t.Updated).HasDefaultValue(DateTime.Now).ValueGeneratedOnAddOrUpdate();

                b.HasMany(t => t.Checks)
                    .WithOne(c => c.TodoItem)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(c => c.TodoItemId)
                    .IsRequired();

            });
            builder.Entity<Check>(b =>
            {
                b.HasKey(c => c.Id);
                b.HasAlternateKey(c => new {c.TodoItemId, c.CheckedDate});
                
            });
        }
    }
}
