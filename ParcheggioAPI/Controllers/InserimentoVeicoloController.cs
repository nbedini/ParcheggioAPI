using Microsoft.AspNetCore.Mvc;
using NLog;
using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParcheggioAPI.Controllers
{

    [ApiController]
    public class InserimentoVeicoloController : Controller
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();

        [HttpGet("/api/datiDB")]
        public ActionResult DatiDB()
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                List<string> tv = new List<string>();

                var VeicoliAttualmenteParcheggiati = model.ParkingStatusses
                    .ToList();

                var ProprietariAttualmenteRegistrati = model.Persons
                    .ToList();

                foreach (var v in model.ParkingCosts)
                {
                    tv.Add(v.TipoVeicolo);
                }

                DatiInserimentoVeicolo datiInserimento = new DatiInserimentoVeicolo
                {
                    ProprietariAttualmenteRegistrati = ProprietariAttualmenteRegistrati,
                    VeicoliAttualmenteParcheggiati = VeicoliAttualmenteParcheggiati,
                    TipiVeicoli = tv
                };

                if (ProprietariAttualmenteRegistrati != null && VeicoliAttualmenteParcheggiati != null && tv != null)
                    return Ok(datiInserimento);
                else
                    return BadRequest();
            }
        }

        [HttpPost("/api/inserimentoveicolo")]
        public ActionResult InserimentoVeicolo([FromBody]InserimentoVeicoloConProprietario inserimentoVeicolo)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                try
                {
                    model.Persons.Add(new Person
                    {
                        Nome = inserimentoVeicolo.Person.Nome,
                        Cognome = inserimentoVeicolo.Person.Cognome,
                        DataNascita = inserimentoVeicolo.Person.DataNascita,
                        CodiceFiscale = inserimentoVeicolo.Person.CodiceFiscale
                    });
                    model.Vehicles.Add(new Vehicle
                    {
                        Marca = inserimentoVeicolo.Veicolo.Marca,
                        Modello = inserimentoVeicolo.Veicolo.Modello,
                        Targa = inserimentoVeicolo.Veicolo.Targa,
                        Propietario = inserimentoVeicolo.Veicolo.Propietario,
                        TipoVeicolo = inserimentoVeicolo.Veicolo.TipoVeicolo
                    });
                    model.ParkingStatusses.Add(new ParkingStatuss
                    {
                        Targa = inserimentoVeicolo.TabellaInserimento.Targa,
                        Riga = inserimentoVeicolo.TabellaInserimento.Riga,
                        Colonna = inserimentoVeicolo.TabellaInserimento.Colonna,
                        DataOrarioEntrata = inserimentoVeicolo.TabellaInserimento.DataOrarioEntrata,
                        NomeParcheggio = inserimentoVeicolo.TabellaInserimento.NomeParcheggio,
                        TipoVeicolo = inserimentoVeicolo.TabellaInserimento.TipoVeicolo
                    });
                    model.SaveChanges();
                    return Ok();
                }
                catch (Exception)
                {
                    return Problem();
                }
            }
        }

        [HttpPost("/api/inserimentoveicolosenzaproprietario")]
        public ActionResult InserimentoVeicolo([FromBody] InserimentoVeicoloSenzaProprietario inserimentoVeicolo)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                try
                {
                    if (model.Vehicles.Where(w => w.Targa == inserimentoVeicolo.Veicolo.Targa).Count() == 0)
                    {
                        model.Vehicles.Add(new Vehicle
                        {
                            Marca = inserimentoVeicolo.Veicolo.Marca,
                            Modello = inserimentoVeicolo.Veicolo.Modello,
                            Targa = inserimentoVeicolo.Veicolo.Targa,
                            Propietario = inserimentoVeicolo.Veicolo.Propietario,
                            TipoVeicolo = inserimentoVeicolo.Veicolo.TipoVeicolo
                        });
                        model.ParkingStatusses.Add(new ParkingStatuss
                        {
                            Targa = inserimentoVeicolo.TabellaInserimento.Targa,
                            Riga = inserimentoVeicolo.TabellaInserimento.Riga,
                            Colonna = inserimentoVeicolo.TabellaInserimento.Colonna,
                            DataOrarioEntrata = inserimentoVeicolo.TabellaInserimento.DataOrarioEntrata,
                            NomeParcheggio = inserimentoVeicolo.TabellaInserimento.NomeParcheggio,
                            TipoVeicolo = inserimentoVeicolo.TabellaInserimento.TipoVeicolo
                        });
                        model.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        model.ParkingStatusses.Add(new ParkingStatuss 
                            { 
                            Targa = inserimentoVeicolo.TabellaInserimento.Targa, 
                            Riga = inserimentoVeicolo.TabellaInserimento.Riga, 
                            Colonna = inserimentoVeicolo.TabellaInserimento.Colonna, 
                            DataOrarioEntrata = inserimentoVeicolo.TabellaInserimento.DataOrarioEntrata,
                            NomeParcheggio = inserimentoVeicolo.TabellaInserimento.NomeParcheggio, 
                            TipoVeicolo = inserimentoVeicolo.TabellaInserimento.TipoVeicolo 
                        });
                        model.SaveChanges();
                        return Ok();
                    }
                }
                catch (Exception)
                {
                    return Problem();
                }
            }
        }
    }
}
