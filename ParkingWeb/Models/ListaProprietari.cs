using ParkingWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingWeb.Models
{
    public class ListaProprietari
    {
        public List<string> CodiciFiscali { get; set; }

        public List<Person> Proprietari
        {
            get
            {
                using(ParkingSystemContext model = new ParkingSystemContext())
                {
                    var persone = model.Persons.Where(o => CodiciFiscali.Contains(o.CodiceFiscale)).ToList();
                    return (List<Person>)persone;
                }
            }
        }
    }
}
