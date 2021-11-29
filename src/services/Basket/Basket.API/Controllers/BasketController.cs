using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly FinalPriceService _priceService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IRepository repository,
            FinalPriceService priceService, 
            IMapper mapper, 
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _priceService = priceService ?? throw new ArgumentNullException(nameof(priceService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpPost]
        [Route("[action]", Name = "checkout")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetCart(basketCheckout.UserName);

            if (basket is null)
                return BadRequest();

            var message = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            message.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(message);                 

            await _repository.RemoveCart(basketCheckout.UserName);
            return Accepted();
        }

        [HttpGet]
        [Route("{userName}", Name = "GetCart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<ActionResult> GetCart(string userName)
        {
            var cart = await _repository.GetCart(userName);               
            return Ok(cart ?? ShoppingCart.Empty(userName));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCart([FromBody] ShoppingCart cart)
        {
            if (cart is null)
                return BadRequest();
            await _priceService.UpdatePrices(cart);
            var result = await _repository.UpdateOrCreateCart(cart);
                              
            return Ok(result);
        }

        [HttpDelete]
        [Route("{userName}", Name="DeleteCart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<IActionResult> Delete(string userName)
        {
            await _repository.RemoveCart(userName);
            return Ok();
        }
    }
}