using Microsoft.AspNetCore.Authorization;
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
    /// Logica di interazione per MainMenu_.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        #region Properties
        static HttpClient client = new HttpClient();
        public bool ParcheggioEsistenteProp { get; set; } = false;
        public bool ParcheggioNuovo { get; set; } = false;
        public bool ParcheggioEsistenteSelezionato { get; set; } = false;
        public bool NuovoParcheggioCreato { get; set; } = false;
        public string NomeParcheggioCreato { get; set; }
        public string ParcheggioScelto { get; set; }
        public bool LogoutEffettuato { get; set; } = false;


        #endregion

        #region Constructor
        public MainMenu(bool admin)
        {
            InitializeComponent();
            if(!admin) 
                btNuovo.Visibility = System.Windows.Visibility.Collapsed;
            else 
                btNuovo.Visibility = System.Windows.Visibility.Visible;
            this.DataContext = this;
        }


        #endregion

        #region Events

        private void ParcheggioNuovoClick(object sender, RoutedEventArgs e)
        {
            ParcheggioNuovo = true;
            NuovoParcheggio NuovoParcheggioView = new NuovoParcheggio();
            NuovoParcheggioView.ShowDialog();
            NomeParcheggioCreato = NuovoParcheggioView.NomeParcheggio;
            NuovoParcheggioCreato = NuovoParcheggioView.ParcheggioCreato;
            if (NuovoParcheggioCreato)
                this.Close();
            else
            {
                ParcheggioNuovo = false;
            }
        }

        private async void ParcheggioEsistenteClick(object sender, RoutedEventArgs e)
        {
            ParcheggioEsistenteProp = true;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Properties.Settings.Token);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:31329/api/Admin"),

            };
            var response = await client.SendAsync(request);
            var risposta = await response.Content.ReadAsStringAsync();
            ParcheggioEsistente ParcheggioEsistenteView = new ParcheggioEsistente(risposta);
            ParcheggioEsistenteView.ShowDialog();
            ParcheggioScelto = ParcheggioEsistenteView.NomeParcheggioSelezionato;
            ParcheggioEsistenteSelezionato = ParcheggioEsistenteView.ParcheggioSelezionato;
            if (ParcheggioEsistenteSelezionato)
                this.Close();
            else
            {
                ParcheggioEsistenteProp = false;
                this.Close();
            }
        }

        #endregion
    }
}
