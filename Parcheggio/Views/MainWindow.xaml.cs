using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;
using Parcheggio.Properties;
using ParcheggioAPI.Models;

namespace Parcheggio.Views
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Properties

        // Dichiarazione delle proprieta' e interfacce.

        public bool LoginCompletato { get; set; } = true;
        public bool CambioParcheggio { get; set; } = false;
        public bool PrimoAvvio { get; set; } = false;
        public bool NienteRicarica { get; set; } = true;
        private string userloggato;

        public string UserLoggato
        {
            get { return userloggato; }
            set 
            { 
                userloggato = value; 
                if (PrimoAvvio) OnPropertyChanged("UserLoggato"); 
            }
        }

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
        public HttpClient client { get; set; } = new HttpClient();

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Costructor

        #region Diverse Fasi

        public void SupportPrimaParte()
        {
            while (LogoutEffettuato)
            {
                // Chiamo il metodo della fase zero per far comparire come prima pagina quella di login.
                Login LoginView = new Login();
                LoginView.ShowDialog();
                LoginCompletato = LoginView.LoginCompletato;
                NienteRicarica = false;
                UserLoggato = LoginView.UsernameLogin;
                AdminYesONo = LoginView.Risposta;
                LoginChiusuraSenzaCompletamento = LoginView.LoginEffettuatoChiusuraForm;
                SwitchLoginRegistrazione = LoginView.SwitchRegistrazione;
                LogoutEffettuato = false;
                if (SwitchLoginRegistrazione)
                {
                    // Se l'utente invece di fare il login si deve registrare gli viene cambiata la pagina visualizzata in quella da registrazione.
                    Registrazione RegistrazioneView = new Registrazione();
                    RegistrazioneView.ShowDialog();
                    UserLoggato = RegistrazioneView.UsernameRegistrato;
                    SwitchRegistrazioneLogin = RegistrazioneView.SwitchLogin;
                    LoginCompletato = RegistrazioneView.StatusChiusura;
                    if (SwitchRegistrazioneLogin)
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
            }
            SupportTerzaParte();
        }

        #endregion

        #region Fase Tre

        /// <summary>
        /// Metodo dell'ultima fase, quest'ultima controlla le variabili che sono state inserite in precedenza per effettuare determinate operazioni.
        /// </summary
        public async Task SupportTerzaParte()
        {
            InitializeComponent();
            if (!AdminYesONo) itProprietari.Visibility = Visibility.Collapsed;
            else itProprietari.Visibility = Visibility.Visible;
            // Chiamate a un metodo asincrono che ottiene dall'API i dati e genera la visualizzazione del parcheggio.
            if (PrimoAvvio) await GenerazioneParcheggio(4, true, LoginCompletato);
            else await GenerazioneParcheggio(1, false, LoginCompletato);
            LoginCompletato = false;
            this.DataContext = this;
        }

        #endregion

        #endregion

        // Costruttore che chiama il metodo della fase uno.
        public MainWindow()
        {
            SupportPrimaParte();
        }

        #region Public Methods

        /// <summary>
        /// Metodo dell'interfaccia che inserito nel set di una variabile permette di aggiornare la view delle modifiche che ha subito quella variabile.
        /// </summary>
        /// <param name="propertyname"> Parametro che corrisponde al nome della proprieta' che deve controllare </param>
        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        /// <summary>
        /// Metodo che durante la creazione visiva del parcheggio viene associato a ogni bottone che rappresenta il parcheggio all'evento click.
        /// </summary>
        /// <param name="sender"> Corrisponde al bottone che e' stato premuto </param>
        public async void AllButtonClick(object sender, EventArgs e)
        {
            StatoParcheggio StatoParcheggioView = new StatoParcheggio(sender, NomeParcheggio);
            StatoParcheggioView.ShowDialog();
            rigastatoparcheggio = StatoParcheggioView.RigaString;
            colonnastatoparcheggio = StatoParcheggioView.ColonnaString;
            ChiusuraStatoParcheggio = StatoParcheggioView.Chiusura;
            ChiusuraStatoParcheggioEsci = StatoParcheggioView.ChiusuraEsci;
            TargaStatoParcheggio = StatoParcheggioView.Targa;

            // Chiamo il metodo che genera il parcheggio per aggiornare la vista da possibili inserimenti o rimozioni.
            if (ChiusuraStatoParcheggio) await GenerazioneParcheggio(3, false, LoginCompletato, rigastatoparcheggio, colonnastatoparcheggio);
            else if (ChiusuraStatoParcheggioEsci) await GenerazioneParcheggio(2, false, LoginCompletato, rigastatoparcheggio, colonnastatoparcheggio);
        }

        public async void NuovoParcheggioClick(object sender, EventArgs e)
        {
            this.Hide();
            bool ParcheggioCreato = false;
            NuovoParcheggio nuovoParcheggioView = new NuovoParcheggio();
            nuovoParcheggioView.ShowDialog();
            NomeParcheggioCreato = nuovoParcheggioView.NomeParcheggio;
            ParcheggioCreato = nuovoParcheggioView.ParcheggioCreato;
            LoginCompletato = false;
            PrimoAvvio = true;
            NomeParcheggio = NomeParcheggioCreato;
            ParcheggioNuovoMenu = true;
            await GenerazioneParcheggio(1, false, LoginCompletato);
            this.Show();
        }
        public async void ParcheggioEsistenteClick(object sender, EventArgs e)
        {
            this.Hide();
            bool ParcheggioSelezionato = false;
            ParcheggioEsistente parcheggioEsistenteView = new ParcheggioEsistente(AdminYesONo);
            parcheggioEsistenteView.ShowDialog();
            ParcheggioEsistenteScelto = parcheggioEsistenteView.NomeParcheggioSelezionato;
            ParcheggioSelezionato = parcheggioEsistenteView.ParcheggioSelezionato;
            LoginCompletato = false;
            PrimoAvvio = true;
            NomeParcheggio = ParcheggioEsistenteScelto;
            ParcheggioEsistenteMenu = true;
            await GenerazioneParcheggio(1, false, LoginCompletato);
            this.Show();
        }

        #endregion

        #region GenerazioneParcheggio usando API(int status, string rigaeliminata, string colonnaeliminata)

        /// <summary>
        /// Metodo che si occupa di contattare l'API e di generare il parcheggio dal lato grafico.
        /// </summary>
        /// Tutti i parametri in ingresso vengono mandati tramite chiamata http all'API.
        public async Task GenerazioneParcheggio(int status, bool cambioparcheggio,bool logineffettuato, string rigaeliminata = "", string colonnaeliminata = "")
        {
            Grid grid = new Grid();
            Grid grid1 = new Grid();
            if (!logineffettuato)
            {
                itProprietari.Visibility = Visibility.Visible;
                Menu.Visibility = Visibility.Visible;
                IncassoStorico.Visibility = Visibility.Visible;
                IncassoGiornaliero.Visibility = Visibility.Visible;
                VeicoliAttuali.Visibility = Visibility.Visible;
                aggiorna.Visibility = Visibility.Visible;
                storico.Visibility = Visibility.Visible;
                // Creo una richiesta di tipo POST e come body gli spedisco un oggetto di tipo DatiParcheggio.
                // Dopo aver mandato la richiesta converto nel tipo di dato che mi interessa la stringa di risposta.
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
                // Imposto dei parametri per la griglia che visualizzara' il parcheggio.
                grid.Background = new SolidColorBrush(Colors.Gray);
                for (int k = 0; k < Convert.ToInt32(data.Righe); k++)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                }
                for (int k = 0; k < Convert.ToInt32(data.Colonne); k++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(120) });
                }
                // Per ogni riga e colonna vado a inserirci un bottone.
                for (int k = 0; k < Convert.ToInt32(data.Righe); k++)
                {
                    for (int j = 0; j < Convert.ToInt32(data.Colonne); j++)
                    {
                        Button AreeParcheggio = new Button();
                        AreeParcheggio.Name = "Button" + data.rigacompleta[k * Convert.ToInt32(data.Colonne)] + data.colonnacompleta[j];
                        AreeParcheggio.Margin = new Thickness(2, 2, 0, 0);
                        AreeParcheggio.Background = new SolidColorBrush(Colors.LimeGreen);
                        AreeParcheggio.Click += new RoutedEventHandler(AllButtonClick);
                        // Controllo per verificare se in quel determinato parcheggio e' presente un veicolo usando questo dizionario passatomi dall'API.
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
                // Per finire importo questa griglia in quella presente nel file XAML.
                    ParcheggioView.Children.Add(grid);
            }
            else
            {
                itProprietari.Visibility = Visibility.Collapsed;
                Menu.Visibility = Visibility.Collapsed;
                IncassoStorico.Visibility = Visibility.Collapsed;
                IncassoGiornaliero.Visibility = Visibility.Collapsed;
                VeicoliAttuali.Visibility = Visibility.Collapsed;
                aggiorna.Visibility = Visibility.Collapsed;
                storico.Visibility = Visibility.Collapsed;
                for (int i = 0; i < 5; i++)
                {
                    grid1.RowDefinitions.Add(new RowDefinition());
                    if(i == 2)
                        grid1.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100)});
                    else
                        grid1.ColumnDefinitions.Add(new ColumnDefinition());
                }
                for (int i = 0; i < 2; i++)
                {
                    if(i == 1)
                    {
                        Button button = new Button();
                        button.Content = "Nuovo Parcheggio";
                        button.Height = 100;
                        button.FontSize = 40;
                        button.Click += new RoutedEventHandler(NuovoParcheggioClick);
                        Grid.SetRow(button, 2);
                        Grid.SetColumn(button, 1);
                        grid1.Children.Add(button);
                    }
                    else
                    {
                        Button button = new Button();
                        button.Content = "Parcheggio Esistente";
                        button.Height = 100;
                        button.FontSize = 40;
                        button.Click += new RoutedEventHandler(ParcheggioEsistenteClick);
                        Grid.SetRow(button, 2);
                        Grid.SetColumn(button, 3);
                        grid1.Children.Add(button);
                    }

                }
                ParcheggioView.Children.Add(grid1);
            }
        }

        #endregion

        #region Menù

        /// <summary>
        /// Metodo che risponde alla premuta del tasto Menu' nella barra di navigazione.
        /// </summary>
        private async void Ritorna_Menu(object sender, RoutedEventArgs e)
        {
            this.Hide();
            PrimoAvvio = true;
            //await SupportSecondaParte();
            await SupportTerzaParte();
            Aggiorna(new { }, new RoutedEventArgs());
            this.Show();
        }

        /// <summary>
        /// Metodo che risponde alla premuta del tasto Veicoli Attuali nella barra di navigazione.
        /// </summary>
        private void Stato_Completo(object sender, RoutedEventArgs e)
        {
            StatoCompleto sc = new StatoCompleto(NomeParcheggio);
            sc.ShowDialog();
        }

        /// <summary>
        /// Metodo che risponde alla premuta del tasto Storico nella barra di navigazione.
        /// </summary>
        private void Storico(object sender, RoutedEventArgs e)
        {
            VisualizzaStorico vs = new VisualizzaStorico(NomeParcheggio);
            vs.ShowDialog();
        }

        /// <summary>
        /// Metodo che risponde alla premuta del tasto Incasso Storico nella barra di navigazione.
        /// </summary>
        private void Incasso_Storico(object sender, RoutedEventArgs e)
        {
            IncassoStorico ig = new IncassoStorico(NomeParcheggio);
            ig.ShowDialog();
        }

        /// <summary>
        /// Metodo che risponde alla premuta del tasto Incasso Attuale nella barra di navigazione.
        /// </summary>
        private void Incasso_Giornaliero(object sender, RoutedEventArgs e)
        {
            IncassoAttuale ig = new IncassoAttuale(NomeParcheggio);
            ig.ShowDialog();
        }

        /// <summary>
        /// Metodo che risponde alla premuta del tasto Aggiorna nella barra di navigazione.
        /// </summary>
        private async void Aggiorna(object sender, RoutedEventArgs e)
        {
            GenerazioneParcheggio(4,false, LoginCompletato);
        }

        /// <summary>
        /// Metodo che risponde alla premuta del tasto Proprietari nella barra di navigazione, visibile solo a chi effettua il login come admin.
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ListaProprietari listaProprietariView = new ListaProprietari();
            listaProprietariView.ShowDialog();
        }

        /// <summary>
        /// Metodo che risponde alla premuta del tasto Logout nella barra di navigazione sotto il proprio nickname.
        /// </summary>
        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Sicuro di voler eseguire il logout ? ", "Conferma logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Post,
                            RequestUri = new Uri("http://localhost:31329/api/Logout"),

                        };
                        var response = await client.SendAsync(request);
                        var risposta = await response.Content.ReadAsStringAsync();
                        Settings.Token = "";
                        this.Hide();
                        PrimoAvvio = true;
                        LogoutEffettuato = true;
                        SupportPrimaParte();
                        Aggiorna(new { }, new RoutedEventArgs());
                        this.Show();
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