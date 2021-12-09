using ParkingWeb.Model;
using System.Collections.Generic;

namespace ParkingWeb.ViewModels
{
    public class IncassiAttualiViewModel
    {
        public ParkingAmount IncassoAttuale { get; set; }
        public List<ParkingHistory> VeicoliUscitiOggi { get; set; }
    }
}
