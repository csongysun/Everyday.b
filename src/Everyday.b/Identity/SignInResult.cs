using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b.Common;
using Everyday.b.Models;

namespace Everyday.b.Identity
{
    public class SignInResult :TaskResult<User>
    {
        public static SignInResult PwdNotCorrect => new SignInResult { Errors = new List<Error> {SignInErrorDescriber.PwdNotCorrect} };
    }

    public static class SignInErrorDescriber
    {
        public static Error PwdNotCorrect => new Error
        {
            Code = nameof(PwdNotCorrect),
            Description = Resource.PwdNotCorrect
        };
    }
}
