using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {   
        public ValidationException(IEnumerable<ValidationFailure> errors)
            : base("one or mode validation errors occured")
        {
            Errors = errors.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(grp => grp.Key, grp => grp.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; } 
    }
}
