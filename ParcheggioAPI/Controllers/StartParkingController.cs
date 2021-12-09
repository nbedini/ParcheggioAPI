using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParcheggioAPI.Models;
using Microsoft.AspNetCore.Authorization;
using NLog;

namespace ParcheggioAPI.Controllers
{
    
    public class StartParkingController : Controller
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        [Route("/api/parkings")]
        public ActionResult GetParkins()
        {
            using(ParkingSystemContext model = new ParkingSystemContext())
            {
                return Ok(model.Parkings.ToList());
            }
            
        }


        [Authorize]
        [HttpGet]
        [Route("/api/parking")]
        public ActionResult GetOneParkingStart([FromBody] string ParkingName)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.Parkings.Any(q => q.NomeParcheggio == ParkingName))
                    return Ok(model.Parkings.Where(q => q.NomeParcheggio == ParkingName).ToList());
                else
                    logger.Log(LogLevel.Error, "Tentata ricerca di parcheggio non esistente");
                    return NotFound("PARCHEGGIO NON TROVATO");
            }
              
        }

        [HttpPost]
        [Route("/api/parking/create")]
        public ActionResult PostNewParking([FromBody] Parking PostParkNew )
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (PostParkNew.NomeParcheggio !=null && PostParkNew.Righe != null && PostParkNew.Colonne != null)
                    if (model.Parkings.Any(q => q.NomeParcheggio == PostParkNew.NomeParcheggio))
                        return Problem("NOME PARCHEGGIO ESISTENTE, CAMBIA NOME");
                    else
                    {
                        model.Parkings.Add(PostParkNew);
                        model.SaveChanges();
                        logger.Log(LogLevel.Error, "Parcheggio {parcheggio} creato correttamente",PostParkNew.NomeParcheggio);
                        return Ok("Parcheggio creato con successo");
                    }
                else return Problem("Controlla di aver inserito i dati correttamente"); 
            }
        }
    }
}
