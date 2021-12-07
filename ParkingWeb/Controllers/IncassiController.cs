using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingWeb.Model;
using ParkingWeb.Models;

namespace ParkingWeb.Controllers
{
    public class IncassiController : Controller
    {
        [HttpGet]
        [Route("/Incassi")]
        public IActionResult Index()
        {
            List<IncassiGornalieri> incassi = new List<IncassiGornalieri>();
            return View(incassi);
        }
    }
}
