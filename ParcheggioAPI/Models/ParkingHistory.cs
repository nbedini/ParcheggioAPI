using System;
using System.Collections.Generic;

#nullable disable

namespace ParcheggioAPI.Models
{
    public partial class ParkingHistory
    {
        public long Idstorico { get; set; }
        public string Targa { get; set; }
        public DateTime DataOrarioEntrata { get; set; }
        public DateTime DataOrarioUscita { get; set; }
        public TimeSpan TempoTrascorso { get; set; }
        public double Tariffa { get; set; }
        public string Riga { get; set; }
        public string Colonna { get; set; }
        public string TipoVeicolo { get; set; }
        public string NomeParcheggio { get; set; }
        public string Propietario { get; set; }

        public virtual Person PropietarioNavigation { get; set; }
        public virtual ParkingCost TipoVeicoloNavigation { get; set; }
    }
}
