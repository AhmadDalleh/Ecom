using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> get()
        {
            try
            {
                var product = await work.ProductRepository
                    .GetAllAsync(x => x.Category, x => x.Photos);
                var result = mapper.Map<List<ProductDto>>(product);
                if(product is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult>getById(int id)
        {
            try
            {
                var product = await work.ProductRepository.GetByIdAsync(id,x=>x.Category,x=> x.Photos);
                var result = mapper.Map<ProductDto>(product);
                if (product is null) return BadRequest(new ResponseAPI(400));
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
    }
}
