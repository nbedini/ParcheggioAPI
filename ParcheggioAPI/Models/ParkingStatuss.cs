using System;
using System.Collections.Generic;

#nullable disable

namespace ParcheggioAPI.Models
{
    public partial class ParkingStatuss
    {
        public string Targa { get; set; }
        public DateTime DataOrarioEntrata { get; set; }
        public string Riga { get; set; }
        public string Colonna { get; set; }
        public string NomeParcheggio { get; set; }
        public string TipoVeicolo { get; set; }

        public virtual Parking NomeParcheggioNavigation { get; set; }
    }
}
