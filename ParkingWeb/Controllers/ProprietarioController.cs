using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingWeb.Models;
using ParkingWeb.Model;
using ParkingWeb.ViewModels;

namespace ParkingWeb.Controllers
{
    public class ProprietarioController : Controller
    {
        [HttpGet("/VisualizzaProprietario/{codiceFiscale}/{NomeParcheggio}/{Identificativo}")]
        public IActionResult Index(string codiceFiscale,string NomeParcheggio,bool Identificativo = true)
        {
            if (String.IsNullOrEmpty(NomeParcheggio)) return Problem("Nome parcheggio non valido");
            if (codiceFiscale.Length > 16 || codiceFiscale.Length < 16) return Problem("Codice fiscale non valido");
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var proprietario = model.Persons.FirstOrDefault(o => o.CodiceFiscale.Equals(codiceFiscale));
                if (proprietario == null) return NotFound("Persona non trovata");

                var macchine = model.Vehicles.Where(o => o.Propietario.Equals(codiceFiscale)).ToList();

                Proprietario_Macchine pm = new Proprietario_Macchine
                {
                    Persona = proprietario,
                    Veicoli = macchine
                };

                ParcheggioDetailsModel pdm = new ParcheggioDetailsModel
                {
                    NomeParcheggio = NomeParcheggio,
                    Proprietario_Macchine = pm,
                    Identificativo = Identificativo
                };

                return View(pdm);
            }
        }


        [HttpGet("/VisualizzaProprietari/{NomeParcheggio}")]
        public IActionResult All(string NomeParcheggio)
        {
            if (String.IsNullOrEmpty(NomeParcheggio)) return Problem("Nome parcheggio non valido");
            using (ParkingSystemContext model = new ParkingSystemContext())
            {

                var targhe = model.ParkingHistorys.Where(o => o.NomeParcheggio.Equals(NomeParcheggio)).Select(o => o.Targa).ToList();
                var proprietari_history = model.ParkingHistorys.Where(o => o.NomeParcheggio.Equals(NomeParcheggio)).Select(o => o.Propietario).ToList();

                List<string> p = new List<string>();

                for(int i=0;i<proprietari_history.Count;i++)
                {
                    if (!p.Contains(proprietari_history[i]))
                        p.Add(proprietari_history[i]);
                }

                ListaProprietari lp = new ListaProprietari
                {
                    CodiciFiscali = p
                };

                ParcheggioDetailsModel pdm = new ParcheggioDetailsModel
                {
                    NomeParcheggio = NomeParcheggio
                };

                persone_detailParcheggio pdp = new persone_detailParcheggio
                {
                    Persone = lp.Proprietari,
                    ParcheggioDetailsModel = pdm
                };

                return View(pdp);
            }
        }




        [HttpGet("/ModificaProprietario/{codiceFiscale}/{NomeParcheggio}")]
        public ActionResult ModificaProprietario(string codiceFiscale,string NomeParcheggio)
        {
            if (String.IsNullOrEmpty(NomeParcheggio)) return Problem("Nome parcheggio non valido");
            if (codiceFiscale.Length > 16 || codiceFiscale.Length < 16) return Problem("Codice fiscale non valido");
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var proprietario = model.Persons.FirstOrDefault(o => o.CodiceFiscale.Equals(codiceFiscale));
                if (proprietario == null) return NotFound("Persona non trovata");

                ParcheggioDetailsModel pdm = new ParcheggioDetailsModel
                {
                    NomeParcheggio = NomeParcheggio,
                    Persona = proprietario
                };

                return View(pdm);
            }
        }


        [HttpPost("/ModificaProprietario/{codiceFiscale}/{NomeParcheggio}")]
        public ActionResult ModificaProprietario(string codiceFiscale,string NomeParcheggio,[FromForm] ParcheggioDetailsModel aggiornamento)
        {
            if (String.IsNullOrEmpty(NomeParcheggio)) return Problem("Nome parcheggio non valido");
            if (aggiornamento.Persona.CodiceFiscale.Length > 16 || aggiornamento.Persona.CodiceFiscale.Length < 16
                || String.IsNullOrEmpty(aggiornamento.Persona.Cognome) || String.IsNullOrEmpty(aggiornamento.Persona.Nome)) return Problem("Dati passati non corretti");

            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var proprietario = model.Persons.FirstOrDefault(o => o.CodiceFiscale.Equals(aggiornamento.Persona.CodiceFiscale));
                if (proprietario == null) return NotFound("Persona non trovata");

                proprietario.CodiceFiscale = aggiornamento.Persona.CodiceFiscale;
                proprietario.Cognome = aggiornamento.Persona.Cognome;
                proprietario.DataNascita = aggiornamento.Persona.DataNascita;
                proprietario.Nome = aggiornamento.Persona.Nome; 

                model.SaveChanges();

                return Redirect($"http://localhost:34483/VisualizzaProprietari/{NomeParcheggio}");
            }
        }
    }
}
