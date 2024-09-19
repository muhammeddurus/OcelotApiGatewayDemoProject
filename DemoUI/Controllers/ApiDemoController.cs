using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DemoUI.Controllers
{
    public class ApiDemoController : Controller
    {
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri("https://localhost:7267/ocelot/api/customer");
                client.BaseAddress = new Uri("https://localhost:7267");
                var response = await  client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var list = await response.Content.ReadFromJsonAsync<List<Customer>>();
                    return View(list);
                }
            }
            return View();
        }
    }
}
