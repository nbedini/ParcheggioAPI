using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        [Route("")]
        public ActionResult CreaUser([FromBody] User utente)
        {

            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (model.Users.Select(s => new { s.Id, s.Username }).FirstOrDefault(i => i.Username == utente.Username) == null)
                {
                    model.Users.Add(utente);
                    model.SaveChanges();
                    return Ok();
                }
                return Problem();
            }
        }
    }
}
