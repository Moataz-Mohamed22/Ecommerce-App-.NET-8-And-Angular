using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<ReturnProductDTO> GetAllAsync(ProductParams productParams);
        public Task<bool> AddAsync(AddProductDTO dto);
        public Task<bool> UpdateAsync(UpdateProductDTO dTO);
        public Task DeleteAsync(Product product);
    }
}
