using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingWeb.Model
{
    public partial class ParkingAmount
    {
        public DateTime Giorno { get; set; }
        public string IncassoTotale { get; set; }
        public string NomeParcheggio { get; set; }

        public virtual Parking NomeParcheggioNavigation { get; set; }
    }
}
