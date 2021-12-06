using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParcheggioAPI.Models;

namespace ParcheggioAPI.Controllers
{
    [ApiController]
    public class EsciVeicoloController : ControllerBase
    {
        [HttpDelete("/api/esciveicolo")]
        protected ActionResult EsciVeicolo([FromBody] OggettoEsciVeicolo o)
        {
            if (o.Veicolo == null) return Problem("Veicolo passato non valido");
            if (o.Veicolo.Targa.Length == 0 || o.Veicolo.Propietario.Length == 0 || o.Veicolo.Targa == null || o.Veicolo.Propietario == null) return Problem("Veicolo passato non valido!");

            DateTime OrarioUscita, OrarioIngresso = new DateTime(); //prendo la data e orario corrente 
            OrarioUscita = DateTime.Now;

            using(ParkingSystemContext model = new ParkingSystemContext())
            {
                //prendo l'orario d'ingresso del veicolo che sta uscendo così calcolo il tempo trascorso
                OrarioIngresso = model.ParkingStatusses
                   .FirstOrDefault(fod => fod.Targa == o.Veicolo.Targa).DataOrarioEntrata;

                TimeSpan TempoTrascorso = OrarioUscita - OrarioIngresso;

                //aggiungo all'history
                model.ParkingHistorys.Add(new ParkingHistory
                {
                    NomeParcheggio = o.NomeParcheggio,
                    TipoVeicolo = o.Veicolo.TipoVeicolo,
                    TempoTrascorso = TempoTrascorso,
                    Colonna = o.Colonna,
                    Riga = o.Riga,
                    DataOrarioEntrata = OrarioIngresso,
                    DataOrarioUscita = OrarioUscita,

                    Propietario = model.Vehicles
                       .FirstOrDefault(fod => fod.Targa == o.Veicolo.Targa).Propietario,
                    Targa = o.Veicolo.Targa,
                    Tariffa = model.ParkingCosts
                       .FirstOrDefault(fod => fod.TipoVeicolo == o.Veicolo.TipoVeicolo).Tariffa
                });

                //salvo
                model.SaveChanges();

                //rimuovo da stato parcheggio
                model.ParkingStatusses
                   .Remove(model.ParkingStatusses
                   .FirstOrDefault(fod => fod.Targa == o.Veicolo.Targa && fod.Riga == o.Riga && fod.Colonna == o.Colonna));


                model.SaveChanges();

                //se trovo già un incasso nel giorno odierno aggiorno senò isnerisco nuovo incasso
                if (model.ParkingAmounts.Where(w => w.NomeParcheggio == o.NomeParcheggio).Count() > 0)
                {
                    var giorno = TimeSpan.FromDays(1) - TimeSpan.FromMilliseconds(1);
                    var candidate = model.ParkingAmounts.FirstOrDefault(fod => fod.NomeParcheggio == o.NomeParcheggio);
                    candidate.IncassoTotale = model.ParkingHistorys
                        .Where(w => w.NomeParcheggio == o.NomeParcheggio && w.TempoTrascorso < giorno)
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
                            .Where(w => w.NomeParcheggio == o.NomeParcheggio && w.TempoTrascorso < giorno)
                            .Sum(s => s.Tariffa)
                            .ToString(),
                        Giorno = DateTime.Today
                    });

                    model.SaveChanges();
                }

                return Ok(TempoTrascorso);
            }
        }

        }

    public class OggettoEsciVeicolo
    {
        public Vehicle Veicolo { get; set; }
        public string NomeParcheggio { get; set; }
        public string Colonna { get; set; }
        public string Riga { get; set; }
    }

}



