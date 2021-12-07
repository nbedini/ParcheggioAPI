using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingWeb.Models;
using ParkingWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace ParkingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/Index/{nomeParcheggio}")]
        public IActionResult Index(string nomeParcheggio)
        {
            ParcheggioDetailsModel pdm = new ParcheggioDetailsModel
            {
                NomeParcheggio = nomeParcheggio
            };
            return View(pdm);
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
    }
}
