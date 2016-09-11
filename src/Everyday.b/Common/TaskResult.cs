using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Everyday.b.Common;

namespace Everyday.b.Common
{
    public class TaskResult
    {
        private readonly List<Error> _errors = new List<Error>();
        public bool Succeeded { get; protected set; }
        public static TaskResult Success { get; } = new TaskResult { Succeeded = true };
        public IEnumerable<Error> Errors => _errors;
        public static TaskResult Failed(params Error[] errors)
        {
            var result = new TaskResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }

    }
}
