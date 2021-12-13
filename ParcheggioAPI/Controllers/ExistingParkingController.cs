using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcheggioAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ExistingParkingController : ControllerBase
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpGet("/api/existing")]
        public ActionResult ExistingParking()
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                List<string> displayParking = model.Parkings.Select(s => s.NomeParcheggio)
                                                            .ToList();
                return Ok(displayParking);
            }
        }

        [HttpDelete("/api/remove")]
        public ActionResult RemoveParking([FromBody] string parking)
        {
            if (parking == null) 
            {
                logger.Log(LogLevel.Error, "Tentata eliminazione di un parcheggio non esistente");
                return Problem("Fornire un parcheggio corretto"); 
            }

            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                Parking removeParking = model.Parkings
                                             .Where(s => s.NomeParcheggio == parking)
                                             .FirstOrDefault();

                if(removeParking == null) { return NotFound(); }

                var autostorico = model.ParkingHistorys.Where(w => w.NomeParcheggio == parking).ToList();
                var autoparcheggiate = model.ParkingStatusses.Where(w => w.NomeParcheggio == parking).ToList();
                var incassistorico = model.ParkingAmounts.Where(w => w.NomeParcheggio == parking).ToList();

                if (autostorico.Count > 0 || autoparcheggiate.Count > 0 || incassistorico.Count > 0)
                {
                    foreach (var v in autostorico)
                    {
                        model.ParkingHistorys.Remove(v);
                        model.SaveChanges();
                    }
                    foreach (var v in autoparcheggiate)
                    {
                        model.ParkingStatusses.Remove(v);
                        model.SaveChanges();
                    }
                    foreach (var v in incassistorico)
                    {
                        model.ParkingAmounts.Remove(v);
                        model.SaveChanges();
                    }
                    if (model.ParkingHistorys.Where(w => w.NomeParcheggio == parking).Count() == 0 && model.ParkingStatusses.Where(w => w.NomeParcheggio == parking).Count() == 0)
                    {
                        model.Parkings.Remove(removeParking);
                        model.SaveChanges();
                        logger.Log(LogLevel.Info, "Eliminazione del parcheggio {parcheggio}", removeParking.NomeParcheggio);
                        return Ok("Parcheggio rimosso con successo");
                    }
                    else
                    {
                        return Problem();
                    }
                }
                else
                {
                    model.Parkings.Remove(removeParking);
                    model.SaveChanges();
                    logger.Log(LogLevel.Info, "Eliminazione del parcheggio {parcheggio}", removeParking.NomeParcheggio);
                    return Ok("Parcheggio rimosso con successo");
                }
            }
        }
    }
}
