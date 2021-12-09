using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParcheggioAPI.Controllers
{
    
    [ApiController]
    public class TempoTariffaController : ControllerBase
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();

        [HttpGet("/api/IncassiStorico/{Name}")]
        public ActionResult Index(string Name)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                if (Name != "")
                {
                    var listaincassi = model.ParkingAmounts.Where(w => w.NomeParcheggio == Name).OrderBy(ob => ob.Giorno).ToList();
                    return Ok(listaincassi);
                }
                else
                    return BadRequest();
            }
        }

        [HttpGet("/api/IncassiAttuali/{nomeparcheggio}")]
        public ActionResult IncassiAttuali(string nomeparcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var incassoodierno = model.ParkingAmounts.FirstOrDefault(fod => fod.Giorno == DateTime.Today && fod.NomeParcheggio == nomeparcheggio);
                var autousciteoggi = model.ParkingHistorys.Where(w => w.DataOrarioUscita.Date == DateTime.Today && w.NomeParcheggio == nomeparcheggio).ToList();
                IncassiAttualiViewModel incassiAttuali = new IncassiAttualiViewModel
                {
                    VeicoliUscitiOggi = autousciteoggi,
                    IncassoAttuale = incassoodierno
                };
                if (incassiAttuali.VeicoliUscitiOggi != null && incassiAttuali.IncassoAttuale != null)
                    return Ok(incassiAttuali);
                else
                    return NotFound();
            }
        }
    }
}

