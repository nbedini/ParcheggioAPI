using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParcheggioAPI.Models;
using System.Linq;

namespace ParcheggioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoricoPacheggioController : ControllerBase
    {
        [HttpGet("/api/storicoparcheggio/{nomeParcheggio}")]
        public ActionResult StoricoParcheggio(string nomeParcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var Veicoli = model.ParkingHistorys.Where(o => o.NomeParcheggio == nomeParcheggio).ToList();
                if (Veicoli.Count() > 0)
                    return Ok(Veicoli);
                else
                    return Problem();
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
