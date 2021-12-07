using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Properties

        public bool PrimoAvvio { get; set; } = false;
        public bool NienteRicarica { get; set; } = true;
        public string UserLoggato { get; set; }
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
        private string nomeparcheggio;

        public string NomeParcheggio
        {
            get { return nomeparcheggio; }
            set 
            { 
                nomeparcheggio = value; 
                if(PrimoAvvio) 
                    OnPropertyChanged("NomeParcheggio"); 
                NomeParcheggioCodeBehind = value; 
            }
        }
        public static string NomeParcheggioCodeBehind { get; set; }
        public string NomeParcheggioCreato { get; set; }
        public bool LogoutEffettuato { get; set; } = true;
        public bool SwitchLoginRegistrazione { get; set; } = false;
        public bool SwitchRegistrazioneLogin { get; set; } = false;
        public bool RegistrazioneEffettuata { get; set; } = false;
        public bool LoginChiusuraSenzaCompletamento { get; set; } = false;

        HttpClient client = new HttpClient();

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Costructor

        public async Task SupportPrimaParte()
        {
            MainMenu MenuView = new MainMenu(AdminYesONo);
            MenuView.ShowDialog();
            LogoutEffettuato = MenuView.LogoutEffettuato;
            ParcheggioEsistenteScelto = MenuView.ParcheggioScelto;
            ParcheggioEsistenteMenu = MenuView.ParcheggioEsistenteProp;
            ParcheggioNuovoMenu = MenuView.ParcheggioNuovo;
            NomeParcheggioCreato = MenuView.NomeParcheggioCreato;
        }

        public async Task SupportSecondaParte()
        {
            if (ParcheggioEsistenteMenu || ParcheggioNuovoMenu)
            {
                if (ParcheggioEsistenteMenu)
                {
                    NomeParcheggio = ParcheggioEsistenteScelto;
                }
                else if (ParcheggioNuovoMenu)
                {
                    NomeParcheggio = NomeParcheggioCreato;
                }
                InitializeComponent();
                if (!AdminYesONo) itProprietari.Visibility = Visibility.Collapsed;
                else itProprietari.Visibility = Visibility.Visible;
                if(PrimoAvvio)
                    await GenerazioneParcheggio(4,true);
                else
                    await GenerazioneParcheggio(1, false);
                this.DataContext = this;

            }
            else
                Application.Current.Shutdown();
        }

        public MainWindow()
        {
            while (LogoutEffettuato)
            {
                Login LoginView = new Login();
                LoginView.ShowDialog();
                NienteRicarica = false;
                UserLoggato = LoginView.UsernameLogin;
                AdminYesONo = LoginView.Risposta;
                LoginChiusuraSenzaCompletamento = LoginView.LoginEffettuatoChiusuraForm;
                SwitchLoginRegistrazione = LoginView.SwitchRegistrazione;
                if (SwitchLoginRegistrazione)
                {
                    Registrazione RegistrazioneView = new Registrazione();
                    RegistrazioneView.ShowDialog();
                    UserLoggato = RegistrazioneView.UsernameRegistrato;
                    SwitchRegistrazioneLogin = RegistrazioneView.SwitchLogin;
                    RegistrazioneEffettuata = RegistrazioneView.StatusChiusura;
                    if (RegistrazioneEffettuata)
                    {
                        SupportPrimaParte();
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
                SupportPrimaParte();
            }
            SupportSecondaParte();
        }

        #endregion

        #region Public Methods

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public async void AllButtonClick(object sender, EventArgs e)
        {
            StatoParcheggio StatoParcheggioView = new StatoParcheggio(sender, NomeParcheggio);
            StatoParcheggioView.ShowDialog();
            rigastatoparcheggio = StatoParcheggioView.RigaString;
            colonnastatoparcheggio = StatoParcheggioView.ColonnaString;
            ChiusuraStatoParcheggio = StatoParcheggioView.Chiusura;
            ChiusuraStatoParcheggioEsci = StatoParcheggioView.ChiusuraEsci;
            TargaStatoParcheggio = StatoParcheggioView.Targa;
            if (ChiusuraStatoParcheggio)
            {
                await GenerazioneParcheggio(3,false,rigastatoparcheggio, colonnastatoparcheggio);
            }
            else if (ChiusuraStatoParcheggioEsci)
            {
                await GenerazioneParcheggio(2,false,rigastatoparcheggio,colonnastatoparcheggio);
            }
        }

        #endregion

        #region GenerazioneParcheggio usando API(int status, string rigaeliminata, string colonnaeliminata)

        public async Task GenerazioneParcheggio(int status, bool cambioparcheggio, string rigaeliminata = "", string colonnaeliminata = "")
        {
            Grid grid = new Grid();

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:31329/api/parcheggioview"),
                Content = new StringContent(JsonConvert.SerializeObject(new DatiParcheggio
                {
                    NomeParcheggio = NomeParcheggio,
                    Status = status,
                    rigaeliminata = rigaeliminata,
                    colonnaeliminata = colonnaeliminata,
                    ParcheggioEsistenteMenu = this.ParcheggioEsistenteMenu,
                    ParcheggioNuovoMenu = this.ParcheggioNuovoMenu,
                    CambioParcheggio = cambioparcheggio
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
        private async void Ritorna_Menu(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PrimoAvvio = true;
            await SupportPrimaParte();
            await SupportSecondaParte();
            Aggiorna(new { }, new RoutedEventArgs());
            this.Show();
            
        }

        private void Stato_Completo(object sender, RoutedEventArgs e)
        {
            StatoCompleto sc = new StatoCompleto(NomeParcheggio);
            sc.ShowDialog();
        }
        private void Storico(object sender, RoutedEventArgs e)
        {
            VisualizzaStorico vs = new VisualizzaStorico(NomeParcheggio);
            vs.ShowDialog();
        }
        private void Incasso_Storico(object sender, RoutedEventArgs e)
        {
            IncassoStorico ig = new IncassoStorico(NomeParcheggio);
            ig.ShowDialog();
        }
        private void Incasso_Giornaliero(object sender, RoutedEventArgs e)
        {
            IncassoAttuale ig = new IncassoAttuale(NomeParcheggio);
            ig.ShowDialog();
        }
        private async void Aggiorna(object sender, RoutedEventArgs e)
        {
            GenerazioneParcheggio(4,false);
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ListaProprietari listaProprietariView = new ListaProprietari();
            listaProprietariView.ShowDialog();
        }
        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Sicuro di voler eseguire il logout ? ", "Conferma logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Properties.Settings.Token);
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Post,
                            RequestUri = new Uri("http://localhost:31329/api/Logout"),

                        };
                        var response = await client.SendAsync(request);
                        var risposta = await response.Content.ReadAsStringAsync();
                        Properties.Settings.Token = "";
                        LogoutEffettuato = true;
                        Application.Current.Shutdown();
                        System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0]);
                        break;
                    }
                case MessageBoxResult.No:
                    {
                        break;
                    }
            }
        }
        #endregion
    }
}