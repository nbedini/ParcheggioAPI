using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParcheggioAPI.Models;

namespace ParcheggioWeb.Controllers
{
    public class ProprietarioController : Controller
    {
        [HttpGet]
        [Route("/VisualizzaProprietario")]
        public IActionResult Index(string CodiceFiscale)
        {
            if (CodiceFiscale == null) return Problem("Codice fiscale attualmente non esistente nel registro dati");
            using(ParkingSystemContext model = new ParkingSystemContext())
            {
                var proprietario = model.Persons.FirstOrDefault(o=>o.CodiceFiscale.Equals(CodiceFiscale));
                if (proprietario == null) return NotFound("Proprietario non trovato");
                return View(proprietario);
            }
        }
    }
}
