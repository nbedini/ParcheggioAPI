using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("/api/existing")]
        public ActionResult ExistingParking()
        {
            //Controllo Autenticazione

            //if() { return Unauthorized("Accesso negato"); }


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
            //Controllo Autenticazione

            //if() { return Unauthorized("Accesso negato"); }

            if (parking == null) { return Problem("Fornire un parcheggio corretto"); }

            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                Parking removeParking = model.Parkings
                                             .Where(s => s.NomeParcheggio == parking)
                                             .FirstOrDefault();

                if(removeParking == null) { return NotFound(); }

                model.Parkings.Remove(removeParking);
                model.SaveChanges();
                return Ok("Parcheggio rimosso con successo");
            }
        }
    }
}
