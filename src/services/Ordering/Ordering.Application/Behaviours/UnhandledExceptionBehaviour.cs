using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger) => _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TResponse> Handle(TRequest request, 
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"unhandled error in mediator pipeline. " +
                    $"Request type {typeof(TRequest)} name {typeof(TRequest).Name}");
                throw;
            }            
        }
    }
}
