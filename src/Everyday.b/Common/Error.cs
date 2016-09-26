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

    public static partial class ErrorDescriber
    {
        public static Error DefaultError => new Error
        {
            Code = nameof(DefaultError),
            Description = Resource.DefaultError
        };
        public static Error ConcurrencyFailure => new Error
        {
            Code = nameof(ConcurrencyFailure),
            Description = Resource.ConcurrencyFailure
        };
        public static Error ModelNotValid => new Error
        {
            Code = nameof(ModelNotValid),
            Description = Resource.ModelNotValid
        };
        public static Error ItemNotFound => new Error
        {
            Code = nameof(ItemNotFound),
            Description = Resource.ItemNotFound
        };
        public static Error CheckNotFound => new Error
        {
            Code = nameof(CheckNotFound),
            Description = Resource.CheckNotFound
        };
        public static Error EntityNotFound => new Error
        {
            Code = nameof(EntityNotFound),
            Description = Resource.EntityNotFound
        };
    }

    
}
