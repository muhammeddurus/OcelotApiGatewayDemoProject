using CustomerAPI.Models;
using Entities.Dto;
using Entities.Models;

namespace CustomerAPI.Services.Interface
{
    public interface ICustomerService
    {
        public List<Customer> GetAllAsync();
        public Task AddAsync(Customer customer);

        CustomerDto LoginCustomer(string email,string password);
    }
}
