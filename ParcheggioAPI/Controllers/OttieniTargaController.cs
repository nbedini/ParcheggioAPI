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
    public class OttieniTargaController : ControllerBase
    {
        [HttpGet("/api/ottienitarga/{riga}/{colonna}/{nomeParcheggio}")]
        public ActionResult OttenimentoTarga(string riga, string colonna, string nomeParcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                ParkingStatuss veicolo = model.ParkingStatusses
                    .FirstOrDefault(fod => fod.Riga == riga && fod.Colonna == colonna && fod.NomeParcheggio == nomeParcheggio);
                if (veicolo != null)
                {
                    return Ok(veicolo.Targa);
                }
                else
                    return NotFound(null);
            }
        }
    }
}
