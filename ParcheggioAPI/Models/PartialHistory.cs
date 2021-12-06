using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcheggioAPI.Models
{
    public partial class ParkingHistory
    {
        public TimeSpan GetTempoTrascorso
        {
            get
            {
                return (DataOrarioUscita - DataOrarioEntrata);
            }
        }
    }
}
