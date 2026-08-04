using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;

namespace Ecommerce.API.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name)
                ).ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();
            CreateMap<AddProductDto, Product>()
                .ForMember(p => p.photos, op => op.Ignore())
                .ReverseMap();
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(p => p.photos, op => op.Ignore())
                .ReverseMap();
        }
    }
}
