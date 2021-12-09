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
    
    [ApiController]
    public class OttieniTargaController : ControllerBase
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpGet("/api/ottienitarga/{riga}/{colonna}/{nomeParcheggio}")]
        public ActionResult OttenimentoTarga(string riga, string colonna, string nomeParcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                ParkingStatuss veicolo = model.ParkingStatusses
                    .FirstOrDefault(fod => fod.Riga == riga && fod.Colonna == colonna && fod.NomeParcheggio == nomeParcheggio);
                if (veicolo != null)
                {
                    return Ok(new PassaggioOggettoVeicolo
                    {
                        Targa = veicolo.Targa,
                        TipoVeicolo = veicolo.TipoVeicolo
                    });
                }
                else
                    return NotFound(null);
            }
        }
    }

    public class PassaggioOggettoVeicolo
    {
        public string Targa { get; set; }
        public string TipoVeicolo { get; set; }
    }
}
