using AutoMapper;
using Ecommerce.API.Helper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var products = await _work.ProductRepository.GetAllAsync(p => p.Category,
p => p.photos);
                var res = _mapper.Map<List<ProductDTO>>(products);
                if (products == null)
                    return BadRequest(new ResponseAPI(404, "Not Found Products"));
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _work.ProductRepository.GetByIdAsync
                    (
                 id,
                 c => c.Category,
                 p => p.photos
                     );

                if (product is null)
                    return NotFound(new ResponseAPI(404, "Not Found This Product"));

                var res = _mapper.Map<ProductDTO>(product);

                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add-Product")]
        public async Task <IActionResult>Add(AddProductDto Dto)
        {
            try
            {
                await _work.ProductRepository.AddAsync(Dto);
                return Ok(new ResponseAPI(200,"Added Product Succssfully"));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.InnerException?.Message ?? ex.Message));
            }
        }

        [HttpPut("update-product")]
        public async Task <IActionResult>Update (UpdateProductDTO dTO)
        {
            try
            {
                await _work.ProductRepository.UpdateAsync(dTO);
                return Ok(new ResponseAPI(200, "Updated Successfully"));
            }
            catch (Exception ex) {
                return BadRequest(new ResponseAPI(400, ex.InnerException?.Message ?? ex.Message));

            }
        }
    }
}
