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
        [HttpGet("/VisualizzaProprietario/{codiceFiscale}/{NomeParcheggio}")]
        public IActionResult Index(string codiceFiscale,string NomeParcheggio)
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
                    Proprietario_Macchine = pm
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

                List<Person> persone = new List<Person>(lp.Proprietari);
                

                return View(persone);
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
                return View(proprietario);
            }
        }


        [HttpPost("/ModificaProprietario/{codiceFiscale}/{NomeParcheggio}")]
        public ActionResult ModificaProprietario(string codiceFiscale,string NomeParcheggio,[FromForm] Person aggiornamento)
        {
            if (String.IsNullOrEmpty(NomeParcheggio)) return Problem("Nome parcheggio non valido");
            if (aggiornamento.CodiceFiscale.Length > 16 || aggiornamento.CodiceFiscale.Length < 16
                || String.IsNullOrEmpty(aggiornamento.Cognome) || String.IsNullOrEmpty(aggiornamento.Nome)) return Problem("Dati passati non corretti");

            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                var proprietario = model.Persons.FirstOrDefault(o => o.CodiceFiscale.Equals(aggiornamento.CodiceFiscale));
                if (proprietario == null) return NotFound("Persona non trovata");

                proprietario.CodiceFiscale = aggiornamento.CodiceFiscale;
                proprietario.Cognome = aggiornamento.Cognome;
                proprietario.DataNascita = aggiornamento.DataNascita;
                proprietario.Nome = aggiornamento.Nome; 

                model.SaveChanges();

                return RedirectToAction("Index", new { codiceFiscale = proprietario.CodiceFiscale});
            }
        }
    }
}
