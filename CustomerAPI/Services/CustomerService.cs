using CustomerAPI.Models;
using CustomerAPI.Services.Interface;
using Entities.Dto;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ESaleDbContext _context;

        public CustomerService(ESaleDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            _context.Add(customer);
            await _context.SaveChangesAsync();
        }

        public List<Customer> GetAllAsync()
        {
            return _context.Customer.ToList();
        }

        public CustomerDto LoginCustomer(string email, string password)
        {
            if(_context.Customer.Any(x => x.Email == email && x.Password == password))
            {
                var customer = _context.Customer.Where(x => x.Email == email && x.Password == password).Include(x=> x.City).FirstOrDefault();

                return new CustomerDto
                {
                    Address = customer.Address,
                    CityName = customer.City.Name,
                    Email = customer.Email,
                    Name = customer.Name,
                    LastName = customer.LastName,
                    Id = customer.ID,
                    Phone = customer.Phone,
                    UserName = customer.UserName
                };
            }
            else
            {
                return null;
            }
        }
    }
}
