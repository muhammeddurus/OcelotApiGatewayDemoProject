using Entities.Dto;
using Entities.Models;
using ProductAPI.Models;
using ProductAPI.Services.Interface;

namespace ProductAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ESaleDbContext _context;

        public CategoryService(ESaleDbContext context)
        {
            _context = context;
        }

        public void AddCategory(Category entity)
        {
            _context.Category.Add(entity);
            _context.SaveChanges();
        }

        public List<CategoryDto> GetList()
        {
            var list = _context.Category.Select(x => new CategoryDto
            {
                Id = x.ID,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl
            }).ToList();
            return list;
        }
    }
}
