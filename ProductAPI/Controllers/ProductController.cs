using App.Metrics;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Services.Interface;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMetrics _metrics;

        public ProductController(IProductService productService, IMetrics metrics)
        {
            _productService = productService;
            _metrics = metrics;
        }
        [HttpGet]
        public ActionResult GetProducts()
        {
            _metrics.Measure.Counter.Increment(RegisterMetrics.GetWeatherForecasts);
            return Ok(_productService.GetAllAsync());
        }

        [HttpGet]
        [Route("products-with-category/{id}")]
        public ActionResult GetProductsWithCategoryId(int id)
        {
            return Ok(_productService.GetListProductWithCategoryId(id));
        }

        [HttpGet]
        [Route("product-with-category/{id}")]
        public ActionResult GetProductWithCategoryId(int id)
        {
            return Ok(_productService.GetProductById(id));
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product entity)
        {
            await _productService.AddAsync(entity);
            return Ok();
        }
    }
}
