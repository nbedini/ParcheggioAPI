using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingWeb.Model
{
    public partial class Parking
    {
        public Parking()
        {
            ParkingHistories = new HashSet<ParkingHistory>();
            ParkingStatusses = new HashSet<ParkingStatuss>();
        }

        public string Righe { get; set; }
        public string Colonne { get; set; }
        public string NomeParcheggio { get; set; }

        public virtual ICollection<ParkingHistory> ParkingHistories { get; set; }
        public virtual ICollection<ParkingStatuss> ParkingStatusses { get; set; }
    }
}
