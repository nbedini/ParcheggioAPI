using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingWeb.Models;
using ParkingWeb.Model;

namespace ParkingWeb.ViewModels
{
    public class ParcheggioDetailsModel
    {
        public string NomeParcheggio { get; set; }

        public Proprietario_Macchine?  Proprietario_Macchine { get; set; }
        public bool Identificativo { get; set; }

        public Person? Persona { get; set; }
    }
}
