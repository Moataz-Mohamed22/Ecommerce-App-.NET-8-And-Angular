using AutoMapper;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _work;
        protected readonly IMapper _mapper;

        

        public BaseController(IUnitOfWork work,IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }
    }
}
