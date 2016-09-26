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
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}