using Discount.api.Entities;
using Discount.api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Discount.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IRepository _repository;

        public DiscountController(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [Route("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var coupon = await _repository.GetCoupon(productName);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Coupon>> CreateCoupon([FromBody] Coupon coupon)
        {
            if (coupon is null)
                return BadRequest();

            var result = await _repository.CreateCoupon(coupon);

            if (!result)
                return BadRequest();

            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            if (coupon is null)
                return BadRequest();

            var result = await _repository.UpdateCoupon(coupon);

            if (!result)
                return BadRequest();

            return Ok(coupon);            
        }

        [HttpDelete]
        [Route("{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteDiscount(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return BadRequest();

            var result = await _repository.DeleteCoupon(productName);

            if (!result)
                return BadRequest();

            return Ok();
        }

    }
}