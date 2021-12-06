using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcheggioAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreaUserController : ControllerBase
    {

        [HttpPost("/api/Crea-Utente")]
        public ActionResult CreaUser([FromBody] User utente)
        {

            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.Users.Select(s => new { s.Id, s.Username }).FirstOrDefault(i => i.Username == utente.Username) == null)
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Log(LogLevel.Debug, "Creazione utente con username {User} alla data {oggi}",utente.Username,DateTime.UtcNow);
                    model.Users.Add(utente);
                    model.SaveChanges();
                    return Ok();
                }
                return Problem();
            }
        }
        [HttpGet("/api/utenti")]
        public ActionResult GetUtenti()
        {
            using(ParkingSystemContext model = new ParkingSystemContext()) 
            {
                List<string> displayParking = model.Users.Select(s=>s.Username).ToList();
                                                           
                return Ok(displayParking);
            }

        }
    }
}
