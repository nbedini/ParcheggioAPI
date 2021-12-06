using Newtonsoft.Json;
using Parcheggio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Parcheggio.Views
{
    /// <summary>
    /// Logica di interazione per StatoParcheggio.xaml
    /// </summary>
    public partial class StatoParcheggio : Window
    {
        public bool ChiusuraEsci { get; set; } = false;
        public bool Chiusura { get; set; } = false;
        public bool ChiusuraControlloTarga { get; set; } = false;
        public string EntraEsci { get; set; }
        public string TitoloPagina { get; set; }
        public string Targa { get; set; }
        public string TipoVeicolo { get; set; }
        public string VeicoloConTarga { get; set; }
        public string CoordinateBottone { get; set; }
        public string RigaString { get; set; }
        public string ColonnaString { get; set; }
        public int Riga { get; set; }
        public int Colonna { get; set; }
        public string ParcheggioSelezionato { get; set; }

        HttpClient client = new HttpClient();

        public StatoParcheggio(object sender, string nomeparcheggio)
        {
            ParcheggioSelezionato = nomeparcheggio;
            CoordinateBottone = ((Button)sender).Name.Substring(6);
            CompilazionePagina();
            InitializeComponent();
            this.DataContext = this;
        }

        public async void CompilazionePagina()
        {
            if (CoordinateBottone.Substring(0, 1) == "0" && CoordinateBottone.Substring(2, 1) == "0")
            {
                RigaString = CoordinateBottone.Substring(1, 1);
                ColonnaString = CoordinateBottone.Substring(3);
            }
            if (CoordinateBottone.Substring(0, 1) == "0" && CoordinateBottone.Substring(2, 1) != "0")
            {
                RigaString = CoordinateBottone.Substring(1, 1);
                ColonnaString = CoordinateBottone.Substring(2);
            }
            if (CoordinateBottone.Substring(0, 1) != "0" && CoordinateBottone.Substring(2, 1) == "0")
            {
                RigaString = CoordinateBottone.Substring(0, 1);
                ColonnaString = CoordinateBottone.Substring(3);
            }
            if (CoordinateBottone.Substring(0, 1) != "0" && CoordinateBottone.Substring(2, 1) != "0")
            {
                RigaString = CoordinateBottone.Substring(0, 2);
                ColonnaString = CoordinateBottone.Substring(2);
            }

            Riga = Convert.ToInt32(CoordinateBottone.Substring(0, 2));
            Colonna = Convert.ToInt32(CoordinateBottone.Substring(2));

            TitoloPagina = $"Parcheggio alla riga {Riga + 1} e colonna {Colonna + 1}";

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:31329/api/ottienitarga/{RigaString}/{ColonnaString}/{ParcheggioSelezionato}"),
            };
            var response = await client.SendAsync(request);
            Targa = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

            if (Targa != null)
            {
                VeicoloConTarga = $"{TipoVeicolo} con targa {Targa.ToUpper()}";
                EntraEsci = "Esci";
            }
            else
            {
                VeicoloConTarga = "Parcheggio vuoto";
                EntraEsci = "Entra";
            }
        }
        
        private async void ConfermaClick(object sender, RoutedEventArgs e)
        {
            if(((Button)sender).Content.ToString() == "Entra")
            {
                ControlloTarga ControlloTargaView = new ControlloTarga(RigaString, ColonnaString, ParcheggioSelezionato);
                ControlloTargaView.ShowDialog();
                ChiusuraControlloTarga = ControlloTargaView.ChiusuraconBottone;
                if (ChiusuraControlloTarga)
                {
                    Chiusura = true;
                    this.Close();
                }
            }
            else if (((Button)sender).Content.ToString() == "Esci")
            {
                MessageBoxResult dialogResult = MessageBox.Show("Sei sicuro di voler uscire?", "Uscita parcheggio", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("http://localhost:31329/api/esciveicolo"),
                        Content = new StringContent(JsonConvert.SerializeObject(new OggettoEsciVeicolo
                        {
                           Riga = RigaString,
                           Colonna = ColonnaString,
                           NomeParcheggio = ParcheggioSelezionato
                        }))
                    };
                    var response = await client.SendAsync(request);
                    var data = JsonConvert.DeserializeObject<TimeSpan>(await response.Content.ReadAsStringAsync());
                    MessageBox.Show($"Tempo trascorso: {data.ToString().Substring(0, data.ToString().IndexOf("."))}", "Tempo trascorso", MessageBoxButton.OK, MessageBoxImage.None);
                    ChiusuraEsci = true;
                    this.Close();
                }
                else if (dialogResult == MessageBoxResult.No)
                {
                }
            }
        }
    }
}
