using Microsoft.AspNetCore.Mvc;
using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParcheggioAPI.Controllers
{
    [ApiController]
    public class ControlloTargaController : Controller
    {
        [HttpGet]
        [Route("/api/checkTarga")]
        public IActionResult ControlloTarga([FromBody] string targaVeicolo)
        {
            //facciamo un controllo sulla lunghezza dei caratteri della targa
            if (targaVeicolo.Length == 7)
            {
                using (ParkingSystemContext model = new ParkingSystemContext())
                {
                    //Verifico se nel DB sia presente un veicolo con la targa riportata
                    string targa = model.Vehicles.FirstOrDefault(f => f.Targa == targaVeicolo).Targa;
                    if (targa == null)
                    {
                        return NotFound("Veicolo non trovato");
                    }

                    //genero il pattern della targa, sappiamo che ha due caratteri
                    //alfabetici, seguiti da 3 caratteri numerici e altri due caratteri alfabetici (es. FF 456 JT)
                    Regex patternTarga = new Regex(@"^[A-Za-z]{2}[0-9]{3}[A-Za-z]{2}");

                    //Definito il pattern della targa Europea, controlliamo se la targa del veicolo lo rispetta
                    if (Regex.IsMatch(targa, patternTarga.ToString()) == true)
                    {
                        return Ok("La targa inserita è corretta e rispetta il pattern Europeo");
                    }
                    else
                    {
                        return Content("La targa riportata non corrisponde al pattern Europeo, probabilmente è una targa personalizzata");
                    }
                }
            }
            else
            {
                return Problem("La targa del veicolo non contiene il numero corretto di caratteri, controllare ");
            }

        }
    }
}
