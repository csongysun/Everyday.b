using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Everyday.b.Models
{
    public class RequestModel
    {
        
    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string UserKey { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Nickname { get; set; }
    }
    public class TokenRefreshModel
    {
        [Required]
        public string refresh_token { get; set; }
    }

    public class TodoItemModel
    {
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
    }
}