using System;
using System.Collections.Generic;

#nullable disable

namespace ParcheggioAPI.Models
{
    public partial class Parking
    {
        public Parking()
        {
            ParkingAmounts = new HashSet<ParkingAmount>();
            ParkingStatusses = new HashSet<ParkingStatuss>();
        }

        public string Righe { get; set; }
        public string Colonne { get; set; }
        public string NomeParcheggio { get; set; }

        public virtual ICollection<ParkingAmount> ParkingAmounts { get; set; }
        public virtual ICollection<ParkingStatuss> ParkingStatusses { get; set; }
    }
}
