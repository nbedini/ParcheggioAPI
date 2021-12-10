using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcheggio.Models
{
    internal class InserimentoVeicoloConProprietario
    {
        public Person Person { get; set; }
        public Vehicle Veicolo { get; set; }
        public ParkingStatuss TabellaInserimento { get; set; }
    }
}
