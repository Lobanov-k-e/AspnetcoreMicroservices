using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persisence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<IEnumerable<OrderDTO>>
    {        
        public string UserName { get; set; }   
       
    }

    public class GetOrderListQueryHandler : IRequestHandler<GetOrdersListQuery, IEnumerable<OrderDTO>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrderListQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<OrderDTO>> Handle(
            GetOrdersListQuery request, 
            CancellationToken cancellationToken = default)
        {
            var orders = await _repository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);          
        }
    }
}
