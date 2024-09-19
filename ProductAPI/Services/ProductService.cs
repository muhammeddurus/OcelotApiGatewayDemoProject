using Entities.Dto;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;
using ProductAPI.Services.Interface;

namespace ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ESaleDbContext _dbContext;

        public ProductService(ESaleDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Product product)
        {
            _dbContext.Product.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public List<ProductDto> GetAllAsync()
        {
            var list = _dbContext.Product.AsQueryable().Include(x => x.Supplier).Include(x => x.Category).Select(x => new ProductDto
            {
                Name = x.Name,
                CategoryName = x.Category.Name,
                Category_ID = x.Category.ID,
                ImageUrlBig = x.ImageUrlBig,
                ImageUrlSmall = x.ImageUrlSmall,
                Price = x.Price,
                Quantity = x.Quantity,
                Status = x.Status,
                Stock = x.Stock,
                SupplierImageUrl = x.Supplier.ImageUrl,
                SupplierName = x.Supplier.CompanyName,
                Supplier_ID = x.Supplier.ID,
                ProductImages = x.ProductImages.Select(y => y.ImageUrl).ToList()
            }).ToList();

            return list;
        }

        public List<ProductDto> GetListProductWithCategoryId(int catId)
        {
            var list = _dbContext.Product.AsQueryable().Where(x => x.Category_ID == catId).Select(x => new ProductDto
            {
                Id = x.ID,
                Name = x.Name,
                CategoryName = x.Category.Name,
                Category_ID = x.Category.ID,
                ImageUrlBig = x.ImageUrlBig,
                ImageUrlSmall = x.ImageUrlSmall,
                Price = x.Price,
                Quantity = x.Quantity,
                Status = x.Status,
                Stock = x.Stock,
                SupplierImageUrl = x.Supplier.ImageUrl,
                SupplierName = x.Supplier.CompanyName,
                Supplier_ID = x.Supplier.ID,
                ProductImages = x.ProductImages.Select(y => y.ImageUrl).ToList()
            }).ToList();
            return list;
        }

        public ProductDto GetProductById(int id)
        {
            var entity = _dbContext.Product.Where(x => x.ID == id).Include(a=> a.Supplier).Include(c => c.Category).Include(y => y.ProductImages).FirstOrDefault();
            var listImages = entity.ProductImages.Select(x => x.ImageUrl).ToList();
            return new ProductDto
            {
                Id = entity.ID,
                Name = entity.Name,
                CategoryName = entity.Category.Name,
                Category_ID = entity.Category.ID,
                ImageUrlBig = entity.ImageUrlBig,
                ImageUrlSmall = entity.ImageUrlSmall,
                Price = entity.Price,
                Quantity = entity.Quantity,
                Status = entity.Status,
                Stock = entity.Stock,
                SupplierImageUrl = entity.Supplier.ImageUrl,
                SupplierName = entity.Supplier.CompanyName,
                Supplier_ID = entity.Supplier.ID,
                ProductImages = listImages
            };
        }
    }
}
