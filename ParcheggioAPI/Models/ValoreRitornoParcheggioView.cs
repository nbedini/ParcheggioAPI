using System.Collections.Generic;

namespace ParcheggioAPI.Models
{
    public class ValoreRitornoParcheggioView
    {
        public string Righe { get; set; }
        public string Colonne { get; set; }
        public List<string> rigacompleta { get; set; } = new List<string>();
        public List<string> colonnacompleta { get; set; } = new List<string>();
        public Dictionary<string, string> keyValues { get; set; } = new Dictionary<string, string>();
    }
}
