using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class CustomerCart
    {
        public CustomerCart()
        {
            
        }
        public CustomerCart(int id)
        {
            Id = id;
            
        }
        public int Id { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
