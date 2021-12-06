using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingWeb.Models;
using ParkingWeb.Model;

namespace ParkingWeb.Controllers
{
    public class ProprietarioController : Controller
    {
        [HttpGet("/VisualizzaProprietario/{codiceFiscale}")]
        public IActionResult Index(string codiceFiscale)
        {
            if (codiceFiscale.Length > 16 || codiceFiscale.Length < 16) return Problem("Codice fiscale non valido");
            using(ParkingSystemContext model = new ParkingSystemContext())
            {
                var proprietario = model.Persons.FirstOrDefault(o => o.CodiceFiscale.Equals(codiceFiscale));
                if (proprietario == null) return NotFound("Persona non trovata");

                var macchine = model.Vehicles.Where(o => o.Propietario.Equals(codiceFiscale)).ToList();

                Proprietario_Macchine pm = new Proprietario_Macchine
                {
                    Persona = proprietario,
                    Veicoli = macchine
                };

                return View(pm);
            }
        }
    }
}
