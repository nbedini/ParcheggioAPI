using System;
using System.Collections.Generic;

#nullable disable

namespace ParcheggioAPI.Models
{
    public partial class Vehicle
    {
        public string Marca { get; set; }
        public string Modello { get; set; }
        public string Targa { get; set; }
        public string Propietario { get; set; }
        public string TipoVeicolo { get; set; }

        public virtual Person PropietarioNavigation { get; set; }
    }
}
