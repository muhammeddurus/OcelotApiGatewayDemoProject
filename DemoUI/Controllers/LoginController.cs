using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DemoUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _jwtToken;

        public LoginController()
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index(string email, string password)
        {
            var result = new CustomerDto();
            var loginCustomer = new Login { UserName = email, Password = "md1256" };
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _jwtToken);
                var response = client.PostAsJsonAsync<Login>(ApiConst.ApiUrl+ $"customer/login/{email}/md1256", loginCustomer).Result;
                result = response.Content.ReadFromJsonAsync<CustomerDto>().Result;
            }
            if (result is not null && result.Name is not null)
            {
                HttpContext.Response.Cookies.Append("loginName", result.Name);
                ViewData["userLogin"] = result.Name;
                HttpContext.Session.Set("UserInformation", Encoding.UTF8.GetBytes(result.Name));
                //ViewBag.Message = Session["UserInformation"].ToString();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }

    }
}
