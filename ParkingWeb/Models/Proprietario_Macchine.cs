using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingWeb.Model;
namespace ParkingWeb.Models
{
    public class Proprietario_Macchine
    {
        public Person Persona{ get; set; }
        public List<Vehicle> Veicoli { get; set; }
    }
}
