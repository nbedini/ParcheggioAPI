using Newtonsoft.Json;
using Parcheggio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    /// Logica di interazione per ControlloTarga.xaml
    /// </summary>
    public partial class ControlloTarga : Window
    {
        public bool ChiusuraconBottone { get; set; } = false;
        public string Targa { get; set; }
        public string NomeParcheggio { get; set; }
        public string Riga { get; set; }
        public string Colonna { get; set; }
        public Person Propietario { get; set; }
        public Vehicle Veicolo { get; set; }
        HttpClient client = new HttpClient();
        public ControlloTarga(string riga, string colonna, string nomeparcheggio)
        {
            Riga = riga;
            Colonna = colonna;
            NomeParcheggio = nomeparcheggio;
            InitializeComponent();
            this.DataContext = this;
        }

        private async void AccettaClick(object sender, RoutedEventArgs e)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:31329/api/checkTarga/{Targa}")
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<DatiControlloTarga>(await response.Content.ReadAsStringAsync());
                Propietario = data.Proprietario;
                Veicolo = data.Veicolo;
                ChiusuraconBottone = true;
                InserimentoVeicolo inserimentoVeicoloView = new InserimentoVeicolo(Riga, Colonna, NomeParcheggio, Propietario, Veicolo);
                inserimentoVeicoloView.ShowDialog();
                this.Close();
            }
            else
            {
                InserimentoVeicolo inserimentoVeicoloView = new InserimentoVeicolo(Riga, Colonna, NomeParcheggio, Targa);
                inserimentoVeicoloView.ShowDialog();
                this.Close();
            }
        }
    }
}
