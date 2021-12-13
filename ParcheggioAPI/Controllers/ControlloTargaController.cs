using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParcheggioAPI.Controllers
{
    [ApiController]
    public class ControlloTargaController : Controller
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpGet]
        [Route("/api/checkTarga/{targaVeicolo}")]
        public IActionResult ControlloTarga(string targaVeicolo)
        {
            string CodiceFiscale = "";
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.Vehicles.FirstOrDefault(fod => fod.Targa == targaVeicolo) != null)
                {
                    CodiceFiscale = model.Vehicles.FirstOrDefault(fod => fod.Targa == targaVeicolo).Propietario;

                    DatiControlloTarga controlloTargaObject = new DatiControlloTarga
                    {
                        Proprietario = model.Persons
                            .Select(s => new Person { CodiceFiscale = s.CodiceFiscale, Cognome = s.Cognome , DataNascita = s.DataNascita , Nome = s.Nome })
                            .FirstOrDefault(fod => fod.CodiceFiscale == CodiceFiscale),
                        Veicolo = model.Vehicles
                            .Select(s => new Vehicle { Marca = s.Marca , Modello = s.Modello , Targa = s.Targa , TipoVeicolo = s.TipoVeicolo })
                            .FirstOrDefault(fod => fod.Targa == targaVeicolo)
                    };
                    return Ok(controlloTargaObject);                    
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
