using AuthAPI.Models;
using CustomerAPI.Services.Interface;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var list = _customerService.GetAllAsync();
            return Ok(list);
        }

        [HttpPost]
        [Route("login/{email}/{password}")]
        public ActionResult LoginResult(string email,string password)
        {
            return Ok(_customerService.LoginCustomer(email, password));
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer(Customer customer)
        {
            await _customerService.AddAsync(customer);
            return Ok();
        }
    }
}
