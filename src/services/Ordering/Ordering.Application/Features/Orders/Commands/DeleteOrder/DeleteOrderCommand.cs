using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persisence;
using Ordering.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderHandler> _logger;
        public DeleteOrderHandler(IOrderRepository repository,
            IMapper mapper,
            ILogger<DeleteOrderHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToRemove = await _repository.GetByIdAsync(request.Id);
            if (orderToRemove is null)
            {
                _logger.LogError($"order with id = {orderToRemove.Id} not found");
                throw new NotFoundException<int>(nameof(orderToRemove), orderToRemove.Id);
            }

             _repository.Delete(orderToRemove);
            await _repository.SaveChangesAsync();
            _logger.LogInformation($"order with id = {orderToRemove.Id} successfully deleted");
            return Unit.Value;
        }

        public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
        {
            public DeleteOrderCommandValidator()
            {
                RuleFor(p => p.Id)
                   .NotNull().WithMessage("{Id} is required.");
            }
        }
    }
}
