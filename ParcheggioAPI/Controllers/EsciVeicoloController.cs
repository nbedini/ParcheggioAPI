using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParcheggioAPI.Models;
using NLog;

namespace ParcheggioAPI.Controllers
{
    [ApiController]
    public class EsciVeicoloController : ControllerBase
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpDelete("/api/esciveicolo")]
        public ActionResult EsciVeicolo([FromBody] OggettoEsciVeicolo o)
        {
            Vehicle Veicolo = MetodiSupporto.OttieniVeicolo(MetodiSupporto.OttenimentoTarga(o.Riga, o.Colonna, o.NomeParcheggio));           

            DateTime OrarioUscita, OrarioIngresso = new DateTime(); //prendo la data e orario corrente 
            OrarioUscita = DateTime.Now;

            using(ParkingSystemContext model = new ParkingSystemContext())
            {
                //prendo l'orario d'ingresso del veicolo che sta uscendo così calcolo il tempo trascorso
                OrarioIngresso = model.ParkingStatusses
                   .FirstOrDefault(fod => fod.Targa == Veicolo.Targa).DataOrarioEntrata;

                TimeSpan TempoTrascorso = OrarioUscita - OrarioIngresso;

                //aggiungo all'history
                model.ParkingHistorys.Add(new ParkingHistory
                {
                    NomeParcheggio = o.NomeParcheggio,
                    TipoVeicolo = Veicolo.TipoVeicolo,
                    Colonna = o.Colonna,
                    Riga = o.Riga,
                    DataOrarioEntrata = OrarioIngresso,
                    DataOrarioUscita = OrarioUscita,

                    Propietario = model.Vehicles
                       .FirstOrDefault(fod => fod.Targa == Veicolo.Targa).Propietario,
                    Targa = Veicolo.Targa,
                    Tariffa = model.ParkingCosts
                       .FirstOrDefault(fod => fod.TipoVeicolo == Veicolo.TipoVeicolo).Tariffa
                });

                //salvo
                model.SaveChanges();

                //rimuovo da stato parcheggio
                model.ParkingStatusses
                   .Remove(model.ParkingStatusses
                   .FirstOrDefault(fod => fod.Targa == Veicolo.Targa && fod.Riga == o.Riga && fod.Colonna == o.Colonna));


                model.SaveChanges();

                //se trovo già un incasso nel giorno odierno aggiorno senò isnerisco nuovo incasso
                if (model.ParkingAmounts.Where(w => w.NomeParcheggio == o.NomeParcheggio && OrarioUscita.Date == w.Giorno.Date).Count() > 0)
                {
                    var giorno = TimeSpan.FromDays(1) - TimeSpan.FromMilliseconds(1);
                    var candidate = model.ParkingAmounts.FirstOrDefault(fod => fod.NomeParcheggio == o.NomeParcheggio && OrarioUscita.Date == fod.Giorno.Date);
                    candidate.IncassoTotale = model.ParkingHistorys
                        .Where(w => w.NomeParcheggio == o.NomeParcheggio)
                        .Sum(s => s.Tariffa)
                        .ToString();

                    model.SaveChanges();
                }
                else
                {
                    var giorno = TimeSpan.FromDays(1) - TimeSpan.FromMilliseconds(1);
                    model.ParkingAmounts.Add(new ParkingAmount
                    {
                        NomeParcheggio = o.NomeParcheggio,
                        IncassoTotale = model.ParkingHistorys
                            .Where(w => w.NomeParcheggio == o.NomeParcheggio && w.DataOrarioUscita.Date == OrarioUscita.Date)
                            .Sum(s => s.Tariffa)
                            .ToString(),
                        Giorno = DateTime.Today
                    });

                    model.SaveChanges();
                }
                logger.Log(LogLevel.Info, "Veicolo con targa {targa} uscito.", Veicolo.Targa);
                return Ok(TempoTrascorso.ToString());
            }
        }
        }

    public class OggettoEsciVeicolo
    {
        public string NomeParcheggio { get; set; }
        public string Colonna { get; set; }
        public string Riga { get; set; }
    }

}



