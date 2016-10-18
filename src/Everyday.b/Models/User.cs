using System;
using System.Collections.Generic;

namespace Everyday.b.Models
{
    public class User :Entity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }

        public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public int AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }


        public List<TodoItem> TodoItems { get; set; }
    }

    public class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
        }
        public string Id { get; set; }
    }
}