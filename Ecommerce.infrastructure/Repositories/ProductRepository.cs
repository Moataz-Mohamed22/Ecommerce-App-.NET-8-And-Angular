using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
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

        public async Task<ReturnProductDTO> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.photos)
                .AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(productParams.Search))
            {
                var searchWords = productParams.Search
                    .ToLower()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(p =>
                    searchWords.All(word =>
                        p.Name.ToLower().Contains(word) ||
                        (p.Description != null &&
                         p.Description.ToLower().Contains(word))
                    )
                );
            }

            // Category Filter
            if (productParams.CategoryId.HasValue)
            {
                query = query.Where(p =>
                    p.CategoryId == productParams.CategoryId.Value);
            }

            // Sorting By Price or Name
            query = productParams.Sort switch
            {
                "PriceAsc" => query.OrderBy(p => p.Price),

                "PriceDesc" => query.OrderByDescending(p => p.Price),

                _ => query.OrderBy(p => p.Name)
            };

            // Pagination Defaults
            productParams.PageNumber =
                productParams.PageNumber > 0
                    ? productParams.PageNumber
                    : 1;

            productParams.PageSize =
                productParams.PageSize > 0
                    ? productParams.PageSize
                    : 3;

            // Total Count BEFORE Skip & Take
            var totalCount = await query.CountAsync();

            // Pagination
            var products = await query
                .Skip(productParams.PageSize *
                      (productParams.PageNumber - 1))
                .Take(productParams.PageSize)
                .ToListAsync();

            // Mapping
            var result = new ReturnProductDTO
            {
                TotalCount = totalCount,
                Products = _mapper.Map<List<ProductDTO>>(products)
            };

            return result;
        }
        public async Task<bool> AddAsync(AddProductDTO dto)
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
