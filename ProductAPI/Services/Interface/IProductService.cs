using Entities.Dto;
using Entities.Models;
using ProductAPI.Models;

namespace ProductAPI.Services.Interface
{
    public interface IProductService
    {
        public List<ProductDto> GetAllAsync();
        public Task AddAsync(Product product);
        List<ProductDto> GetListProductWithCategoryId(int catId);
        ProductDto GetProductById(int id);
    }
}
