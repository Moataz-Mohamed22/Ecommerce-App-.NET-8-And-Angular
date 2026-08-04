using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;

namespace Ecommerce.API.Mapping
{
    public class CategoryMapping:Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
        }
    }
}
