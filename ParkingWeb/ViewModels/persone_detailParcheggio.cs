using ParkingWeb.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingWeb.ViewModels
{
    public class persone_detailParcheggio
    {
        public IEnumerable<Person> Persone { get; set; }

        public ParcheggioDetailsModel ParcheggioDetailsModel { get; set; }
    }
}
