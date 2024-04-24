using dbapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dbapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public DbController(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        [HttpGet(Name = "GetCustomers")]
        public IEnumerable<Customer> GetGetCustomers()
        {
            //_dbContext.Customers.ToList();

            return _dbContext.Customers.ToList();
        }


        [HttpPost(Name = "AddCustomer")]
        public string AddCustomer(Customer customer)
        {
            _dbContext.Customers.Add(customer);
            var id = _dbContext.SaveChanges();

            return id.ToString();
        }








    }
}
