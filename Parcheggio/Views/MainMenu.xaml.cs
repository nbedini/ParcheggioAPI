using Parcheggio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool ParcheggioEsistenteProp { get; set; } = false;
        public bool ParcheggioNuovo { get; set; } = false;
        public bool ParcheggioEsistenteSelezionato { get; set; } = false;
        public bool NuovoParcheggioCreato { get; set; } = false;
        public string NomeParcheggioCreato { get; set; }
        public string ParcheggioScelto { get; set; }

        ParcheggioEsistente ParcheggioEsistenteView = new ParcheggioEsistente();

        #endregion

        #region Constructor

        public MainMenu()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        #endregion

        #region Events

        private void ParcheggioNuovoClick(object sender, RoutedEventArgs e)
        {
            ParcheggioNuovo = true;
            this.Hide();
            NuovoParcheggio NuovoParcheggioView = new NuovoParcheggio();
            NuovoParcheggioView.ShowDialog();
            NomeParcheggioCreato = NuovoParcheggioView.NomeParcheggio;
            NuovoParcheggioCreato = NuovoParcheggioView.ParcheggioCreato;
            if (NuovoParcheggioCreato)
                this.Close();
            else
            {
                ParcheggioNuovo = false;
                this.Close();
            }
        }

        private void ParcheggioEsistenteClick(object sender, RoutedEventArgs e)
        {
            ParcheggioEsistenteProp = true;
            this.Hide();
            //ParcheggioEsistente ParcheggioEsistenteView = new ParcheggioEsistente();
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

        private void AccessoClick(object sender, RoutedEventArgs e)
        {
            RegistrazioneLogin window = new RegistrazioneLogin();
            window.Show(); 
        }
    }
}
