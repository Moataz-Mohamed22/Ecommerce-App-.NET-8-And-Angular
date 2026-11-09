using Ecommerce.Core.Entities.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTO
{
    public record ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual List<PhotoDTO> photos { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }

        public string CategoryName { get; set; } 
    }

    public record PhotoDTO
    {
        public string ImageName { get; set; } =string.Empty;
        public int ProductId { get; set; }
    }
    public record AddProductDTO
    {
       
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryId { get; set; }
        public IFormFileCollection Photo { get; set; }
    }
    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
    public record ReturnProductDTO
    {
        public List<ProductDTO> Products { get; set; }
        public int TotalCount { get; set; }

    }
}
