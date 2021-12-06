using System;
using System.Collections.Generic;

#nullable disable

namespace ParkingWeb.Model
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            ParkingHistories = new HashSet<ParkingHistory>();
        }

        public string Marca { get; set; }
        public string Modello { get; set; }
        public string Targa { get; set; }
        public string Propietario { get; set; }
        public string TipoVeicolo { get; set; }

        public virtual Person PropietarioNavigation { get; set; }
        public virtual ICollection<ParkingHistory> ParkingHistories { get; set; }
    }
}
