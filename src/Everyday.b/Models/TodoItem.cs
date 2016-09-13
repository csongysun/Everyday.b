using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everyday.b.Models
{
    public class TodoItem : Entity
    {
        public string Title { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Check> Checks { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }
    }

    public class Check :Entity
    {
        public DateTime CheckedDate { get; set; }
        public string Comment { get; set; }
    }
}
