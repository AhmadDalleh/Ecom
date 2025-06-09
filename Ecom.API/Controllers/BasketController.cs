using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var result = await work.CustomerBasketRepository.GetBasketAsync(id);
            if (result is null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(result);
        }

        [HttpPost("update-basket")]
        public async Task<IActionResult> add(CustomerBasket basket)
        {
            var _basket = await work.CustomerBasketRepository.UpdateBasketAsync(basket);
            return Ok(_basket);
        }
        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> delete(string id)
        {
            var result = await work.CustomerBasketRepository.DeleteBasketAsync(id);
            return result ? Ok(new ResponseAPI(200, "item deleted"))
                : BadRequest(new ResponseAPI(400));
        }


    }
}
