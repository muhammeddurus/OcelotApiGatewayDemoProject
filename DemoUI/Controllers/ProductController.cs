using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly string _jwtToken;

        public ProductController()
        {
            var jwtToken = "";
            var user = new User { Username = "admin", Password = "test" };
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync<User>(ApiConst.ApiUrl + "auth/sign-in-product",user).Result;
                var jwt = response.Content.ReadFromJsonAsync<JWTModel>().Result;
                jwtToken = jwt.AccessToken;
            }
            _jwtToken = jwtToken;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var entity = new ProductDto();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _jwtToken);

                var response = await client.GetAsync(ApiConst.ApiUrl + $"product/product-with-category/{id}");
                entity = await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            return View(entity);
        }
    }
}
