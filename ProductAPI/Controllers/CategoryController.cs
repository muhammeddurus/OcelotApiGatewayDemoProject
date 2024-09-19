using Entities.Models;
using Gozcu.Common.Hub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProductAPI.Services.Interface;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IHubContext<GozcuHub> _hubContext;

        public CategoryController(ICategoryService categoryService, IHubContext<GozcuHub> hubContext)
        {
            _categoryService = categoryService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public ActionResult GetList()
        {
            return Ok(_categoryService.GetList());
        }
        
        [HttpPost]
        public async Task<ActionResult> AddCategoryAsync(Category entity)
        {
            _categoryService.AddCategory(entity);
            await _hubContext.Clients.All.SendAsync("receiveMessage",entity);
            return Ok();
        }
    }
}
