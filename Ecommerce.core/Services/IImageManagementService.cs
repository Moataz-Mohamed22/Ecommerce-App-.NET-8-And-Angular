using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Services
{
    public interface IImageManagementService
    {
        Task<List<string>>AddImageAsync(IFormFileCollection files ,string src);
        void DeletImageAsync (string src);
    }
}
