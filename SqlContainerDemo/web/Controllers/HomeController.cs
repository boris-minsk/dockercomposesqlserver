using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using web.Models;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static readonly HttpClient _httpClient = new HttpClient();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var customer = new Customer();
            customer.AllCustomers = new List<Customer>();
            customer.AllCustomers.AddRange(GetAllCusotmers());
            return View(customer);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {

            var json = JsonConvert.SerializeObject(customer);
            using StringContent jsonContent = new StringContent(json,
        Encoding.UTF8,
            "application/json");

            var response = _httpClient.PostAsync(
                "http://host.docker.internal:58441/api/Db",
                jsonContent).Result;

            var alldata = GetAllCusotmers();

            return RedirectToAction("Index", "Home");
            //return View("Index", alldata);
            //http://localhost:58441/WeatherForecast

        }


        //[HttpGet]
        public List<Customer> GetAllCusotmers()
        {

            var lst = new List<Customer>();
            try
            {
                var content = _httpClient.GetAsync("http://host.docker.internal:58441/api/Db").Result;
                var json = content.Content.ReadAsStringAsync().Result;
                lst =JsonConvert.DeserializeObject<List<Customer>>(json);
            }
            catch (Exception ex)
            {

                
            }

            return lst;
            
        }


    }
}
