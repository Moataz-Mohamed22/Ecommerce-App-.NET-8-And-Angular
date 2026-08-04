using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories.Services
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider _provider;
        public ImageManagementService(IFileProvider provider)
        {
            this._provider = provider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var SaveImageSrc =new List<string>();
            var ImageDirectory = Path.Combine("wwwroot", "Images", src);
            if (!Directory.Exists(ImageDirectory))
            {
                Directory.CreateDirectory(ImageDirectory);

            }
            foreach (var file in files) {
                if(file.Length > 0)
                {
                    var ImageName =file.FileName;
                    var ImagePath = $"Images/{src}/{ImageName}";
                    var root = Path.Combine(ImageDirectory,ImageName);
                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(ImagePath);
                }
            }
            return SaveImageSrc;
        }

        public void DeletImageAsync(string src)
        {
            var info = _provider.GetFileInfo(src);

            if (info.Exists && info.PhysicalPath != null)
            {
                File.Delete(info.PhysicalPath);
            }
        }
    }
}
