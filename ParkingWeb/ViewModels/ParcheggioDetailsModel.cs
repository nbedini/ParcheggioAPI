using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingWeb.Models;

namespace ParkingWeb.ViewModels
{
    public class ParcheggioDetailsModel
    {
        public string NomeParcheggio { get; set; }

        public Proprietario_Macchine?  Proprietario_Macchine { get; set; }
    }
}
