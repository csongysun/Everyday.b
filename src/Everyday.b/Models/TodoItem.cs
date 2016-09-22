using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Everyday.b.Models
{
    public class TodoItem : Entity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public List<Check> Checks { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Check :Entity
    {
        public DateTime CheckedDate { get; set; }
        public bool Checked { get; set; } = false;
        public string Comment { get; set; }
        public string TodoItemId { get; set; }
        public TodoItem TodoItem { get; set; }
    }
}
