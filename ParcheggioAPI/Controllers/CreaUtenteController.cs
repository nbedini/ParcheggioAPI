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
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpPost("/api/Crea-Utente")]
        public ActionResult CreaUser([FromBody] User utente)
        {

            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.Users.Select(s => new { s.Id, s.Username }).FirstOrDefault(i => i.Username == utente.Username) == null)
                {
                    logger.Log(LogLevel.Info, "Creazione utente con username {User}.",utente.Username);
                    string a = logger.Name;
                    model.Users.Add(utente);
                    model.SaveChanges();
                    return Ok(a);
                }
                logger.Log(LogLevel.Fatal, "Tentata creazione utente con username {User} già utilizzato.", utente.Username);
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
