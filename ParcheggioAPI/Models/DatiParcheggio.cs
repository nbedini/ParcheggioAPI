namespace ParcheggioAPI.Models
{
    public class DatiParcheggio
    {
        public string NomeParcheggio { get; set; }
        public int Status { get; set; }
        public string rigaeliminata { get; set; }
        public string colonnaeliminata { get; set; }

        public bool ParcheggioEsistenteMenu { get; set; }
        public bool ParcheggioNuovoMenu { get; set; }
        public bool CambioParcheggio { get; set; }
    }
}
