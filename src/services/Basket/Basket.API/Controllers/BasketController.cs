using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services;
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

        public BasketController(IRepository repository, FinalPriceService priceService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _priceService = priceService ?? throw new ArgumentNullException(nameof(priceService));
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
            var result = await _repository.UpdateCart(cart);
            if (result is null)
                return BadRequest("Cart Not Found");

            await _priceService.UpdatePrices(result);

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