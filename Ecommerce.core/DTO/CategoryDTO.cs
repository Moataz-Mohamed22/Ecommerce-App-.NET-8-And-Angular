using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTO
{
    public record CategoryDTO
    (string Name, string Description);
    public record UpdateCategoryDto
    (string Name, string Description,int Id);
}
