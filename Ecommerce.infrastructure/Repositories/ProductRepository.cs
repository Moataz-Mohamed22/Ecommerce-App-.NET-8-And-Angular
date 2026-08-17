using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Ecommerce.Infrastructure.Repositories.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this._context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.photos)
                .AsNoTracking();
            // Filtering by word
            if (!string.IsNullOrEmpty(productParams.Search))

            {
                //    query = query.Where(m =>
                //        m.Name.ToLower().Contains(productParams.Search.ToLower()) ||
                //        m.Description.ToLower().Contains(productParams.Search.ToLower())

                var searchWords = productParams.Search
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(m =>
                    searchWords.All(word =>
                        m.Name.ToLower().Contains(word) ||
                        m.Description.ToLower().Contains(word)
                    )
                );
            }
            // Filtering by Category
            if (productParams.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == productParams.CategoryId);
            }

            // Sorting
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAsc" => query.OrderBy(p => p.Price),

                    "PriceDesc" => query.OrderByDescending(p => p.Price),

                    _ => query.OrderBy(p => p.Name)
                };
            }
            else
            {
                query = query.OrderBy(p => p.Name);
            }

            // Pagination
            productParams.PageNumber =
                productParams.PageNumber > 0
                ? productParams.PageNumber
                : 1;

            productParams.PageSize =
                productParams.PageSize > 0
                ? productParams.PageSize
                : 3;

            query = query
                .Skip(productParams.PageSize * (productParams.PageNumber - 1))
                .Take(productParams.PageSize);

            // Execute query
            var products = await query.ToListAsync();

            // Mapping
            var result = _mapper.Map<List<ProductDTO>>(products);

            return result;
        }
        public async Task<bool> AddAsync(AddProductDto dto)
        {
            if (dto == null)
                return false;

            var product = _mapper.Map<Product>(dto);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var imagePaths = await _imageManagementService.AddImageAsync(dto.Photo, dto.Name);

            var photos = imagePaths.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            });

            await _context.Photos.AddRangeAsync(photos);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateAsync(UpdateProductDTO dTO)
        {
            if (dTO is null)
                return false;

            var product = await _context.Products
                .Include(c => c.Category)
                .Include(c => c.photos)
                .FirstOrDefaultAsync(p => p.Id == dTO.Id);
            if (product == null)
                return false;
            _mapper.Map(dTO, product);
            var Findphoto = await _context.Photos.Where(m => m.ProductId == dTO.Id).ToListAsync();

            foreach (var item in Findphoto)
            {
                _imageManagementService.DeletImageAsync(item.ImageName);
            }
            _context.Photos.RemoveRange(Findphoto);
            var imagePath = await _imageManagementService.AddImageAsync(dTO.Photo, dTO.Name);

            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = dTO.Id

            }).ToList();
            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(Product product)
        {
            var photo = await _context.Photos.Where(m => m.ProductId == product.Id).ToListAsync();

            foreach (var item in photo)
            {
                _imageManagementService.DeletImageAsync(item.ImageName);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
