using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Everyday.b.Common
{
    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public static class ErrorDescriber
    {
        public static Error DefaultError()
        {
            return new Error
            {
                Code = nameof(DefaultError),
                Description = Resource.DefaultError
            };
        }
        public static Error ConcurrencyFailure()
        {
            return new Error
            {
                Code = nameof(ConcurrencyFailure),
                Description = Resource.ConcurrencyFailure
            };
        }
    }

    
}
