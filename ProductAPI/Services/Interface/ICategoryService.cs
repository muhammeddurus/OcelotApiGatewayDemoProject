using Entities.Dto;
using Entities.Models;

namespace ProductAPI.Services.Interface
{
    public interface ICategoryService
    {
        List<CategoryDto> GetList();
        void AddCategory(Category entity);
        
    }
}
