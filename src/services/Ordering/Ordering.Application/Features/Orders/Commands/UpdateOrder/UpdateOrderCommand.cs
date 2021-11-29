using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persisence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Common;
using Ordering.Application.Mappings;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest, IMapFrom<Order>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public AdressDTO BillingAdress { get; set; }
        public PaymentDTO Payment { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateOrderCommand, Order>();
        }
    }

    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderHandler> _logger;

        public UpdateOrderHandler(IOrderRepository repository, 
            IMapper mapper,
            ILogger<UpdateOrderHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {            
            var orderToUpdate = await _repository.GetByIdAsync(request.Id);
            if (orderToUpdate is null)
            {
                _logger.LogError($"Order with id = {orderToUpdate.Id} not found");
                throw new NotFoundException<int>(nameof(orderToUpdate), orderToUpdate.Id);
            }

            _mapper.Map<UpdateOrderCommand, Order>(request, orderToUpdate);

            _repository.Update(orderToUpdate);
            await _repository.SaveChangesAsync();

            _logger.LogInformation($"Urder with id = {orderToUpdate.Id} successfully updated");

            return Unit.Value;
        }       
    }

    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(p => p.UserName)
               .NotEmpty().WithMessage("{UserName} is required.")
               .NotNull()
               .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress)
               .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
        }
    }
}
