using AutoMapper;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using StackExchange.Redis;


namespace Ecommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService,IConnectionMultiplexer redis)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context, _mapper, _imageManagementService);
            PhotoRepository = new PhotoRepository(_context);
            CustomerCartRepository = new CustomerCartRepository(redis);
        }

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public ICustomerCartRepository CustomerCartRepository { get; }
    }
}
