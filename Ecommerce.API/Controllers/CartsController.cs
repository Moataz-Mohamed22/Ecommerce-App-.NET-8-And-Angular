using AutoMapper;
using Ecommerce.API.Helper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : BaseController
    {
        public CartsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-cart-item/{id}")]
        public async Task<IActionResult> GetCartItem(string id)
        {
            var cart = await _work.CustomerCartRepository.GetCartAsync(id);
            if (cart == null)
            {
                return Ok(new CustomerCart());
            }
            return Ok(cart);
        }
        [HttpPost("update-cart-item/{id}")]
        public async Task<IActionResult> UpdateCartItem([FromBody] CustomerCart cart)
        {
            var updatedCart = await _work.CustomerCartRepository.UpdatedCartAsync(cart);
            if (updatedCart == null)
            {
                return NotFound();
            }
            return Ok(updatedCart);
        }
        [HttpDelete("delete-cart-item/{id}")]
        public async Task<IActionResult> DeleteCartItem(string id)
        {
            var res = await _work.CustomerCartRepository.DeleteCartAsync(id);
            return res ? Ok(new ResponseAPI(200, "Item deleted successfully")) : NotFound(new ResponseAPI(404, "Item not found"));
        }
    }
    }
