using Microsoft.AspNetCore.Mvc;
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
        //Liste di dati Contenuti nel DB
        public List<ParkingStatuss> VeicoliAttualmenteParcheggiati { get; set; }

        private List<Person> proprietariattualmenteregistrati;

        public List<Person> ProprietariAttualmenteRegistrati
        {
            get { return proprietariattualmenteregistrati; }
            set { proprietariattualmenteregistrati = value; }
        }


        [HttpPut("api/inserisciveicolo")]
        public IActionResult Inserimento([FromBody] OggettoEntrata o)
        {
            //Facciamo una verifica dei dati inseriti del Proprietario e del veicolo, che vogliono parcheggiare la macchina
            //Verifichiamo che i dati forniti siano completi
            if (o.Proprietario.Nome != null && o.Proprietario.Cognome != null && o.Proprietario.CodiceFiscale != null && o.Veicolo.Marca != null && o.Veicolo.Modello != null && o.Veicolo.Targa != null)
            {
                //Scarichiamo i dati dal DB
                VeicoliAttualmenteParcheggiati = MetodiSupporto.RecuperoDatiDB(out proprietariattualmenteregistrati);

                //Controlliamo se il DB non ha scaricato nulla
                if(VeicoliAttualmenteParcheggiati == null || ProprietariAttualmenteRegistrati == null)
                {
                    return NotFound();
                }

                using (ParkingSystemContext model = new ParkingSystemContext())
                {
                    //Facciamo un ulteriore controllo sui veicoli attualmente parcheggiati
                    //Lo stesso veicolo non può parcheggiare in più posti contemporaneamente
                    foreach (ParkingStatuss p in VeicoliAttualmenteParcheggiati)
                    {
                        if (p.Targa == o.Veicolo.Targa)
                        {
                            return Problem("Impossibile inserire due volte la stessa targa nel parcheggio");
                        }
                    }
                    //Facciamo un ulteriore controllo sui Proprietari attualmente presenti nel DB
                    foreach (Person person in ProprietariAttualmenteRegistrati)
                    {
                        //Caso in cui il proprietario sia presente nel DB
                        if (person.CodiceFiscale == o.Proprietario.CodiceFiscale)
                        {
                            //Caso in cui la targa del veicolo, quindi il veicolo non sia presente nel DB dei veicoli
                            if (model.Vehicles.Where(w => w.Targa == o.Veicolo.Targa).Count() == 0)
                            {
                                model.Vehicles
                                    .Add(new Vehicle { Marca = o.Veicolo.Marca, Modello = o.Veicolo.Modello, Targa = o.Veicolo.Targa, Propietario = o.Proprietario.CodiceFiscale, TipoVeicolo = o.Veicolo.TipoVeicolo });
                                model.ParkingStatusses
                                    .Add(new ParkingStatuss { Targa = o.Veicolo.Targa, Riga = o.RigaSelezionata, Colonna = o.ColonnaSelezionata, DataOrarioEntrata = DateTime.Now, NomeParcheggio = o.NomeParcheggioSelezionato, TipoVeicolo = o.Veicolo.TipoVeicolo });
                                model.SaveChanges();
                            }
                            else //Caso in cui la targa del veicolo è presente nel DB dei Veicoli
                            {
                                model.ParkingStatusses
                                    .Add(new ParkingStatuss { Targa = o.Veicolo.Targa, Riga = o.RigaSelezionata, Colonna = o.ColonnaSelezionata, DataOrarioEntrata = DateTime.Now, NomeParcheggio = o.NomeParcheggioSelezionato, TipoVeicolo = o.Veicolo.TipoVeicolo });
                                model.SaveChanges();
                            }

                            return Ok();
                        }
                    }

                    //Inserimento dei dati nel DB: Caso in cui il Proprietario non sia registrato nel DB
                    Person persona = new Person { Nome = o.Proprietario.Nome, Cognome = o.Proprietario.Cognome, DataNascita = o.Proprietario.DataNascita, CodiceFiscale = o.Proprietario.CodiceFiscale };

                    model.Persons
                        .Add(persona);
                    model.Vehicles
                        .Add(new Vehicle { Marca = o.Veicolo.Marca, Modello = o.Veicolo.Modello, Targa = o.Veicolo.Targa, Propietario = o.Proprietario.CodiceFiscale, TipoVeicolo = o.Veicolo.TipoVeicolo });
                    model.ParkingStatusses
                            .Add(new ParkingStatuss { Targa = o.Veicolo.Targa, Riga = o.RigaSelezionata, Colonna = o.ColonnaSelezionata, DataOrarioEntrata = DateTime.Now, NomeParcheggio = o.NomeParcheggioSelezionato, TipoVeicolo = o.Veicolo.TipoVeicolo });
                    model.SaveChanges();

                    return Ok();
                }
            }
            else
            {
                return Problem("Non sono stati inseriti i dati in modo corretto");
            }
        }



        //Creiamo l'oggetto OggettoEntrata che conterrà i dati del parcheggio selezionato, e del Proprietario del veicolo con il relativo mezzo
        
            
    }
    public class OggettoEntrata
    {
        public string NomeParcheggioSelezionato { get; set; }
        public string ColonnaSelezionata { get; set; }
        public string RigaSelezionata { get; set; }
        public Person Proprietario { get; set; }
        public Vehicle Veicolo { get; set; }
    }
}
