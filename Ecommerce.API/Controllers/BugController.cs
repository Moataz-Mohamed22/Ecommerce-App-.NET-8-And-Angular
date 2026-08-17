using AutoMapper;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("not-found")]
        public async Task<IActionResult> GetNotFound()
        {
           var Category = await _work.CategoryRepository.GetByIdAsync(100);
            if(Category == null)return NotFound();
            return Ok(Category);
        } 
        [HttpGet("server-error")]
        public async Task<IActionResult> GetServerError()
        {
           var Category = await _work.CategoryRepository.GetByIdAsync(100);
            Category.Name="";
            return Ok(Category);
        } 
        [HttpGet("bad-request/{id}")]
        public async Task<IActionResult> GetBadRequest(int id)
        {
           
            return Ok();
        } 
        [HttpGet("bad-request/")]
        public async Task<IActionResult> GetBadRequest()
        {
           
            return BadRequest();
        } 
    }
}
