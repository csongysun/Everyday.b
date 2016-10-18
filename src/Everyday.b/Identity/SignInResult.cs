using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Identity
{
    public static class SignInResult
    {
        public static TaskResult PwdNotCorrect => TaskResult.Failed(ErrorDescriber.PwdNotCorrect);
        public static TaskResult ValidateFailed => TaskResult.Failed(ErrorDescriber.ValidateFailed);
        public static TaskResult<User> Success(User user) => TaskResult<User>.Success(user);
    }


}

namespace Everyday.b.Common
{
    public static partial class ErrorDescriber
    {
        public static Error PwdNotCorrect => new Error
        {
            Code = nameof(PwdNotCorrect),
            Description = Resource.PwdNotCorrect
        };
        public static Error ValidateFailed => new Error
        {
            Code = nameof(ValidateFailed),
            Description = Resource.ValidateFailed
        };
    }

}
    

