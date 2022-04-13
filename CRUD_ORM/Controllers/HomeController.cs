using CRUD_ORM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace CRUD_ORM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fibonacci(int numero)
        {
            string azureurl = 
                $"https://fiboesxguiabe.azurewebsites.net/api/FiboHttp?code=aygatIU8aC5UOOmDGcZlWn2t0y5NvHkPOPoaIM5bajHrYXJtI5/QLQ==&numero={numero}";
            try
            {
                using (var client = new HttpClient())
                {

                    FiboNumero fiboModel = new FiboNumero();
                    var stringContent = new StringContent(JsonConvert.SerializeObject(fiboModel), Encoding.UTF8, "application/json");                    
                    var result = await client.PostAsync(azureurl, stringContent);
                    fiboModel.resultContent = await result.Content.ReadAsStringAsync();
                    
                    return View("Privacy", fiboModel);
                }
            }
            catch (HttpRequestException ex)
            {
                return Content(ex.Message);
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
