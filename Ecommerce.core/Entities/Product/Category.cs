using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities.Product
{
    public class Category:BaseEntity<int>
    {
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Product> products { get; set; } = new HashSet<Product>();
    }
}
