using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Count() > 0)
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task
                    .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var errors = validationResults
                    .Where(e => e.Errors != null)
                    .SelectMany(e => e.Errors)                    
                    .ToList();

                if (errors.Count() > 0)
                    throw new ValidationException(errors);
            }

           return await next();
        }
    }
}
