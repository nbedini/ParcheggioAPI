using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ParcheggioAPI.Models;
using System.Linq;

namespace ParcheggioAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class StoricoPacheggioController : ControllerBase
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpGet("/api/storicoparcheggio/{nomeParcheggio}")]
        public ActionResult StoricoParcheggio(string nomeParcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var Veicoli = model.ParkingHistorys.Where(o => o.NomeParcheggio == nomeParcheggio).ToList();
                return Ok(Veicoli);
            }
        }

        [HttpPost("/api/storicoricercatarga")]
        public ActionResult StoricoRicercaTarga([FromBody] RicercaTarga targaeparcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var filtro = model.ParkingHistorys.Where(o => o.Targa.Contains(targaeparcheggio.targaparziale) && o.NomeParcheggio == targaeparcheggio.NomeParcheggio).ToList();
                if (filtro != null)
                    return Ok(filtro);
                else
                    return NotFound();
            }
        }
    }
}
