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
        private string dbhost = string.Empty;
        private readonly IConfiguration Configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;

            Configuration = configuration;
            dbhost = GetUrl();
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
                $"{dbhost}/api/Db",
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
                var content = _httpClient.GetAsync($"{dbhost}/api/Db").Result;
                var json = content.Content.ReadAsStringAsync().Result;
                lst = JsonConvert.DeserializeObject<List<Customer>>(json);
            }
            catch (Exception ex)
            {


            }

            return lst;

        }



        string GetUrl()
        {
            string url = string.Empty;
            var ENVIRONMENT_FLAG = Configuration["ENVIRONMENT_FLAG"];
            if (ENVIRONMENT_FLAG == "Development")
            {
                return $"http://{Configuration["DB_API_URL"]}:{Configuration["HTTP_PORT"]}";

            }

            if (ENVIRONMENT_FLAG == "Production")
            {
                return $"https://{Configuration["DB_API_URL"]}:{Configuration["HTTP_PORT"]}";

            }

            return url;


        }

    }
}
