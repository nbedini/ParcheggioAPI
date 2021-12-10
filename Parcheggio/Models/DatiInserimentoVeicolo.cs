using System.Collections.Generic;

namespace Parcheggio.Models
{
    public class DatiInserimentoVeicolo
    {
        public List<ParkingStatuss> VeicoliAttualmenteParcheggiati { get; set; }
        public List<Person> ProprietariAttualmenteRegistrati{ get; set; }
        public List<string> TipiVeicoli { get; set; }
    }
}
