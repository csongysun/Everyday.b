using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Identity
{
    public class SignInResult :TaskResult<User>
    {
        public static SignInResult PwdNotCorrect => new SignInResult { Errors = new List<Error> { ErrorDescriber.PwdNotCorrect} };
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
    }
}
    

