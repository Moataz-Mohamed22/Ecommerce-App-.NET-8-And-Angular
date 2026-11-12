using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface ICustomerCartRepository
    {
        Task<CustomerCart> GetCartAsync(string customerId);
        Task<CustomerCart> UpdatedCartAsync ( CustomerCart cart);
        Task<bool> DeleteCartAsync(string customerId);
    }
}
