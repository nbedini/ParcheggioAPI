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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Parcheggio.Models;
using ParcheggioAPI.Models;

namespace Parcheggio.Views
{
    public partial class MainWindow : Window
    {
        #region Properties

        public bool Status { get; set; } = false;
        public bool AdminYesONo { get; set; } 
        public string TargaStatoParcheggio { get; set; }
        public bool ChiusuraStatoParcheggioEsci { get; set; } = false;
        public string rigastatoparcheggio { get; set; }
        public string colonnastatoparcheggio { get; set; }
        public bool ChiusuraStatoParcheggio { get; set; } = false;
        public bool ParcheggioEsistenteMenu { get; set; } = false;
        public bool ParcheggioNuovoMenu { get; set; } = false;
        public string ParcheggioEsistenteScelto { get; set; }
        public static string NomeParcheggioScelto { get; set; }
        public string NomeParcheggioCreato { get; set; }
        public bool LogoutEffettuato { get; set; } = true;
        public bool SwitchLoginRegistrazione { get; set; } = false;
        public bool SwitchRegistrazioneLogin { get; set; } = false;
        public bool RegistrazioneEffettuata { get; set; } = false;
        public bool LoginChiusuraSenzaCompletamento { get; set; } = false;

        HttpClient client = new HttpClient();

        #endregion

        #region Costructor

        public void Support()
        {
            MainMenu MenuView = new MainMenu(AdminYesONo);
            MenuView.ShowDialog();
            LogoutEffettuato = MenuView.LogoutEffettuato;
            ParcheggioEsistenteScelto = MenuView.ParcheggioScelto;
            ParcheggioEsistenteMenu = MenuView.ParcheggioEsistenteProp;
            ParcheggioNuovoMenu = MenuView.ParcheggioNuovo;
            NomeParcheggioCreato = MenuView.NomeParcheggioCreato;
        }

        public MainWindow()
        {
            while (LogoutEffettuato)
            {
                Login LoginView = new Login();
                LoginView.ShowDialog();
                AdminYesONo = LoginView.Risposta;
                LoginChiusuraSenzaCompletamento = LoginView.LoginEffettuatoChiusuraForm;
                SwitchLoginRegistrazione = LoginView.SwitchRegistrazione;
                if (SwitchLoginRegistrazione)
                {
                    Registrazione RegistrazioneView = new Registrazione();
                    RegistrazioneView.ShowDialog();
                    SwitchRegistrazioneLogin = RegistrazioneView.SwitchLogin;
                    RegistrazioneEffettuata = RegistrazioneView.StatusChiusura;
                    if (RegistrazioneEffettuata)
                    {
                        Support();
                    }
                    else if (SwitchRegistrazioneLogin)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (!LoginChiusuraSenzaCompletamento)
                {
                    break;
                }
                Support();
            }
            if (ParcheggioEsistenteMenu || ParcheggioNuovoMenu)
            {
                if (ParcheggioEsistenteMenu)
                {
                    NomeParcheggioScelto = ParcheggioEsistenteScelto;
                }
                else if (ParcheggioNuovoMenu)
                {
                    NomeParcheggioScelto = NomeParcheggioCreato;
                }
                InitializeComponent();
                GenerazioneParcheggio(1);
                this.DataContext = this;
            }
            else
                Application.Current.Shutdown();
        }

        #endregion

        #region Public Methods

        public void AllButtonClick(object sender, EventArgs e)
        {
            StatoParcheggio StatoParcheggioView = new StatoParcheggio(sender, NomeParcheggioScelto);
            StatoParcheggioView.ShowDialog();
            rigastatoparcheggio = StatoParcheggioView.RigaString;
            colonnastatoparcheggio = StatoParcheggioView.ColonnaString;
            ChiusuraStatoParcheggio = StatoParcheggioView.Chiusura;
            ChiusuraStatoParcheggioEsci = StatoParcheggioView.ChiusuraEsci;
            TargaStatoParcheggio = StatoParcheggioView.Targa;
            if (ChiusuraStatoParcheggio)
            {
                GenerazioneParcheggio(3,rigastatoparcheggio, colonnastatoparcheggio);
            }
            else if (ChiusuraStatoParcheggioEsci)
            {
                GenerazioneParcheggio(2,rigastatoparcheggio,colonnastatoparcheggio);
            }
        }

        #endregion

        #region GenerazioneParcheggio usando API(int status, string rigaeliminata, string colonnaeliminata)

        public async void GenerazioneParcheggio(int status, string rigaeliminata = "", string colonnaeliminata = "")
        {
            Grid grid = new Grid();

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:31329/api/parcheggioview"),
                Content = new StringContent(JsonConvert.SerializeObject(new DatiParcheggio
                {
                    NomeParcheggio = NomeParcheggioScelto,
                    Status = status,
                    rigaeliminata = rigaeliminata,
                    colonnaeliminata = colonnaeliminata,
                    ParcheggioEsistenteMenu = this.ParcheggioEsistenteMenu,
                    ParcheggioNuovoMenu = this.ParcheggioNuovoMenu
                }), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<ValoreRitornoParcheggioView>(await response.Content.ReadAsStringAsync());

            grid.Background = new SolidColorBrush(Colors.Gray);
            for (int k = 0; k < Convert.ToInt32(data.Righe); k++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }
            for (int k = 0; k < Convert.ToInt32(data.Colonne); k++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(120) });
            }
            for (int k = 0; k < Convert.ToInt32(data.Righe); k++)
            {
                for (int j = 0; j < Convert.ToInt32(data.Colonne); j++)
                {
                    Button AreeParcheggio = new Button();
                    AreeParcheggio.Name = "Button" + data.rigacompleta[k * Convert.ToInt32(data.Colonne)] + data.colonnacompleta[j];
                    AreeParcheggio.Margin = new Thickness(2, 2, 0, 0);
                    AreeParcheggio.Background = new SolidColorBrush(Colors.LimeGreen);
                    AreeParcheggio.Click += new RoutedEventHandler(AllButtonClick);
                    foreach (var v in data.keyValues)
                    {
                        if (AreeParcheggio.Name == $"Button{v.Key}")
                        {
                            AreeParcheggio.Background = new SolidColorBrush(Colors.Red);
                            AreeParcheggio.FontSize = 14;
                            AreeParcheggio.FontFamily = new FontFamily("Verdana");
                            AreeParcheggio.Content = v.Value;
                        }
                    }
                    Grid.SetRow(AreeParcheggio, k);
                    Grid.SetColumn(AreeParcheggio, j);
                    grid.Children.Add(AreeParcheggio);
                }
            }
            ParcheggioView.Children.Add(grid);

        }

            #endregion

        #region menù
        private void Ritorna_Menu(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Stato_Completo(object sender, RoutedEventArgs e)
        {
            StatoCompleto sc = new StatoCompleto(NomeParcheggioScelto);
            sc.ShowDialog();
        }
        private void Storico(object sender, RoutedEventArgs e)
        {
            VisualizzaStorico vs = new VisualizzaStorico(NomeParcheggioScelto);
            vs.ShowDialog();
        }
        private void Incasso_Storico(object sender, RoutedEventArgs e)
        {
            
        }
        private void Incasso_Giornaliero(object sender, RoutedEventArgs e)
        {
            IncassoGiornaliero ig = new IncassoGiornaliero(NomeParcheggioScelto);
            ig.ShowDialog();
        }
        private void Aggiorna(object sender, RoutedEventArgs e)
        {
            GenerazioneParcheggio(4);
        }
        #endregion

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ListaProprietari listaProprietariView = new ListaProprietari();
            listaProprietariView.ShowDialog();
        }
    }
}