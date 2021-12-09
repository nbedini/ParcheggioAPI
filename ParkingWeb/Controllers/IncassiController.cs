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
        public IActionResult Index(string searchName)
        {
            ViewData["NomeParcheggioDetails"] = searchName;

            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                IncassiGornalieri listaIncassi = new IncassiGornalieri();
                listaIncassi.Incassi = model.ParkingAmounts.ToList();
                if (!String.IsNullOrEmpty(searchName))
                {
                    listaIncassi.Incassi = listaIncassi.Incassi.Where(w => w.NomeParcheggio.Contains(searchName)).ToList();
                }
            
                return View(listaIncassi);
            }
        }
    }
}
