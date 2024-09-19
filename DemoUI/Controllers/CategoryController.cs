using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Net;

namespace DemoUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly string _jwtToken;
        public CategoryController()
        {
            var jwtToken = "";
            var user = new User { Username = "admin", Password = "test" };
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync<User>(ApiConst.ApiUrl + "auth/sign-in-product", user).Result;
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
            if (id is null || id == 0)
            {
                return BadRequest();
            }
            else
            {
                var list = new List<ProductDto>();
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _jwtToken);
                    var response = await client.GetAsync(new Uri(ApiConst.ApiUrl + $"product/products-with-category/{id}"));
                    list = await response.Content.ReadFromJsonAsync<List<ProductDto>>();    
                }
                return View(list);
            }
        }
    }
}
