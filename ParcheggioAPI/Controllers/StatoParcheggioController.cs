using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ParcheggioAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ParcheggioAPI.Controllers
{
    [ApiController]
    public class StatoParcheggioController : ControllerBase
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpPost("/api/statocompletogrid")]
        public ActionResult StatoCompletoGrid([FromBody]string NomeParcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var AutoParcheggiate = model.ParkingStatusses.Where(o => o.NomeParcheggio == NomeParcheggio).ToList();
                if(AutoParcheggiate != null)
                    return Ok(AutoParcheggiate);
                else
                    return NotFound();
            }
        }

        [HttpPost("/api/statocompletoricercatarga")]
        public ActionResult StatoCompletoRicercaTarga([FromBody] RicercaTarga targaeparcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var filtro = model.ParkingStatusses.Where(o => o.Targa.Contains(targaeparcheggio.targaparziale) && o.NomeParcheggio == targaeparcheggio.NomeParcheggio).ToList();
                if (filtro != null)
                    return Ok(filtro);
                else
                    return NotFound();
            }
        }
    }
}
