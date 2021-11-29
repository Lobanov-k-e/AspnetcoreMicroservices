using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persisence;
using Ordering.Application.Features.Orders.Common;
using Ordering.Application.Mappings;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<int>, IMapFrom<Order>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public AdressDTO BillingAdress { get; set; } = new AdressDTO();
        public PaymentDTO Payment { get; set; } = new PaymentDTO();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CheckoutOrderCommand, Order>();
        }
    }

    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository repository,
            IMapper mapper,
            IEmailService emailService,
            ILogger<CheckoutOrderCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
         
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken = default)
        {
            var order = _mapper.Map<Order>(request);
            var createdOrder = _repository.Add(order);
            await _repository.SaveChangesAsync();

            _logger.LogInformation($"Order {createdOrder.Id} was sucessfully created");

            await SendEmail(createdOrder);

            return createdOrder.Id;
        }

        private async Task SendEmail(Order order)
        {
            var email = new EmailMessage()
            {
                Body = $"Order {order.Id} was sucessfully placed",
                Subject = "Oder placed",
                To = "--"
            };

            try
            {
                await _emailService.Send(email);
            }
            catch (Exception e)
            {
                _logger.LogError($"can't send email for order {order.Id}, error :{e.Message}");
            }
        }
    }


    public class CheckoutCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutCommandValidator()
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
