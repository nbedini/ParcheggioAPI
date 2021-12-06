using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcheggioAPI.Controllers
{
    [ApiController]
    public class TempoTariffaController : ControllerBase
    {
        [HttpGet]
        [Route("/IncassoGiornaliero")]
        public ActionResult GetIncassoGiornaliero()
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.ParkingAmounts.Any())
                    return Ok(model.ParkingAmounts.ToList());
                else
                    return NotFound("Nessun Incasso Trovato");
            }
        }
        [HttpGet]
        [Route("/IncassoGiornaliero/Nome/{NomeParcheggio}")]
        public ActionResult GetOneIncassoGiornaliero_Nome(string NomeParcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.ParkingAmounts.Any(q => q.NomeParcheggio == NomeParcheggio))
                    return Ok(model.ParkingAmounts.Where(q => q.NomeParcheggio == NomeParcheggio).ToList());
                else
                    return NotFound("Nessun Incasso trovato per questo parcheggio");
            }
        }
        [HttpGet]
        [Route("/IncassoGiornaliero/Giorno/{Giorno}")]
        public ActionResult GetOneIncassoGiornaliero_Giorno(DateTime Giorno)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.ParkingAmounts.Any(q => q.Giorno.Day == Giorno.Day))
                    return Ok(model.ParkingAmounts.Where(q => q.Giorno.Day == Giorno.Day).ToList());
                else
                    return NotFound("Nessun Incasso trovato per questo Giorno");
            }
        }
        [HttpGet]
        [Route("/IncassoGiornaliero/Nome/{NomeParcheggio}/Giorno/{Giorno}")]
        public ActionResult GetOneIncassoGiornaliero(string NomeParcheggio, DateTime Giorno)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.ParkingAmounts.Any(q => q.Giorno.Day == Giorno.Day && q.NomeParcheggio == NomeParcheggio))
                    return Ok(model.ParkingAmounts.Where(q => q.Giorno.Day == Giorno.Day && q.NomeParcheggio == NomeParcheggio).ToList());
                else
                    return NotFound("Nessun Incasso trovato per questo Parcheggio");
            }
        }


    }
}

