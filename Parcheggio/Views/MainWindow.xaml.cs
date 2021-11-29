using System;
using System.Collections.Generic;
using System.Linq;
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
using Parcheggio.Models;

namespace Parcheggio.Views
{
    public partial class MainWindow : Window
    {
        #region Properties

        public string TargaStatoParcheggio { get; set; }
        public bool ChiusuraStatoParcheggioEsci { get; set; } = false;
        public string rigastatoparcheggio { get; set; }
        public string colonnastatoparcheggio { get; set; }
        public bool ChiusuraStatoParcheggio { get; set; } = false;
        public bool ParcheggioEsistenteMenu { get; set; } = false;
        public bool ParcheggioNuovoMenu { get; set; } = false;
        public string ParcheggioEsistenteScelto { get; set; }
        public string NomeParcheggioScelto { get; set; }

        Dictionary<string, string> keyValues = new Dictionary<string, string>();

        #endregion

        #region Costructor

        public MainWindow()
        {
            MainMenu MenuView = new MainMenu();
            MenuView.ShowDialog();
            ParcheggioEsistenteScelto = MenuView.ParcheggioScelto;
            ParcheggioEsistenteMenu = MenuView.ParcheggioEsistenteProp;
            ParcheggioNuovoMenu = MenuView.ParcheggioNuovo;
            if (ParcheggioEsistenteMenu || ParcheggioNuovoMenu)
            {
                if (ParcheggioEsistenteMenu)
                {
                    NomeParcheggioScelto = ParcheggioEsistenteScelto;
                }
                else if (ParcheggioNuovoMenu)
                {
                    NomeParcheggioScelto = MenuView.NomeParcheggioCreato;
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

        public List<ParkingStatuss> OttenimentoDatiDB()
        {
            using (ParkingSystemEntities model = new ParkingSystemEntities())
            {
                return model.ParkingStatusses
                            .Where(w => w.NomeParcheggio == NomeParcheggioScelto)
                            .ToList();
            }
        }

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

        #region OttenimentoAutoParcheggiate(int status, string rigaeliminata, string colonnaeliminata)

        public Dictionary<string, string> OttenimentoAutoParcheggiate(int status, string rigaeliminata, string colonnaeliminata)
        {
            var VeicoliParcheggiati = OttenimentoDatiDB();
            string riga = "", colonna = "";

            if (status == 1)
            {
                #region Controllo Righe e Colonne

                foreach (var v in VeicoliParcheggiati)
                {
                    if (Convert.ToInt32(v.Riga) < 10 && Convert.ToInt32(v.Colonna) >= 10)
                    {
                        riga = "0" + v.Riga;
                        colonna = v.Colonna;
                    }
                    if (Convert.ToInt32(v.Riga) >= 10 && Convert.ToInt32(v.Colonna) < 10)
                    {
                        riga = v.Riga;
                        colonna = "0" + v.Colonna;
                    }
                    if (Convert.ToInt32(v.Riga) < 10 && Convert.ToInt32(v.Colonna) < 10)
                    {
                        riga = "0" + v.Riga;
                        colonna = "0" + v.Colonna;
                    }
                    if (Convert.ToInt32(v.Riga) >= 10 && Convert.ToInt32(v.Colonna) >= 10)
                    {
                        riga = v.Riga;
                        colonna = v.Colonna;
                    }
                    keyValues.Add(riga + colonna, v.Targa);
                }

                #endregion 

                return keyValues;
            }
            else if(status == 2)
            {
                #region Controllo Righe e Colonne

                if (Convert.ToInt32(rigaeliminata) < 10 && Convert.ToInt32(colonnaeliminata) >= 10)
                {
                    riga = "0" + rigaeliminata;
                    colonna = colonnaeliminata;
                }
                if (Convert.ToInt32(rigaeliminata) >= 10 && Convert.ToInt32(colonnaeliminata) < 10)
                {
                    riga = rigaeliminata;
                    colonna = "0" + colonnaeliminata;
                }
                if (Convert.ToInt32(rigaeliminata) < 10 && Convert.ToInt32(colonnaeliminata) < 10)
                {
                    riga = "0" + rigaeliminata;
                    colonna = "0" + colonnaeliminata;
                }
                if (Convert.ToInt32(rigaeliminata) >= 10 && Convert.ToInt32(colonnaeliminata) >= 10)
                {
                    riga = rigaeliminata;
                    colonna = colonnaeliminata;
                }

                #endregion

                keyValues.Remove(riga + colonna);
                return keyValues;
            }
            else if (status == 3)
            {
                #region Controllo Righe e Colonne

                if (Convert.ToInt32(rigaeliminata) < 10 && Convert.ToInt32(colonnaeliminata) >= 10)
                {
                    riga = "0" + rigaeliminata;
                    colonna = colonnaeliminata;
                }
                if (Convert.ToInt32(rigaeliminata) >= 10 && Convert.ToInt32(colonnaeliminata) < 10)
                {
                    riga = rigaeliminata;
                    colonna = "0" + colonnaeliminata;
                }
                if (Convert.ToInt32(rigaeliminata) < 10 && Convert.ToInt32(colonnaeliminata) < 10)
                {
                    riga = "0" + rigaeliminata;
                    colonna = "0" + colonnaeliminata;
                }
                if (Convert.ToInt32(rigaeliminata) >= 10 && Convert.ToInt32(colonnaeliminata) >= 10)
                {
                    riga = rigaeliminata;
                    colonna = colonnaeliminata;
                }

                #endregion

                using (ParkingSystemEntities model = new ParkingSystemEntities())
                {
                    if(model.ParkingStatusses.FirstOrDefault(fod => fod.Riga == rigaeliminata && fod.Colonna == colonnaeliminata) != null)
                    {
                        string targa = model.ParkingStatusses.FirstOrDefault(fod => fod.Riga == rigaeliminata && fod.Colonna == colonnaeliminata).Targa;
                        keyValues.Add(riga + colonna, targa);
                        return keyValues;
                    }
                    else
                    {
                        return keyValues;
                    }
                }
            }
            else if (status == 4)
            {
                keyValues = new Dictionary<string, string>();

                #region Controllo Righe e Colonne

                foreach (var v in VeicoliParcheggiati)
                {
                    if (Convert.ToInt32(v.Riga) < 10 && Convert.ToInt32(v.Colonna) >= 10)
                    {
                        riga = "0" + v.Riga;
                        colonna = v.Colonna;
                    }
                    if (Convert.ToInt32(v.Riga) >= 10 && Convert.ToInt32(v.Colonna) < 10)
                    {
                        riga = v.Riga;
                        colonna = "0" + v.Colonna;
                    }
                    if (Convert.ToInt32(v.Riga) < 10 && Convert.ToInt32(v.Colonna) < 10)
                    {
                        riga = "0" + v.Riga;
                        colonna = "0" + v.Colonna;
                    }
                    if (Convert.ToInt32(v.Riga) >= 10 && Convert.ToInt32(v.Colonna) >= 10)
                    {
                        riga = v.Riga;
                        colonna = v.Colonna;
                    }
                    keyValues.Add(riga + colonna, v.Targa);
                }

                #endregion

                return keyValues;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region GenerazioneParcheggio(int status, string rigaeliminata, string colonnaeliminata)

        public void GenerazioneParcheggio(int status, string rigaeliminata = "", string colonnaeliminata = "")
        {
            Grid grid = new Grid();
            var Righe = "";
            var Colonne = "";

            #region Ottenimento righe e colonne dal database

            using (ParkingSystemEntities model = new ParkingSystemEntities())
            {
                if (ParcheggioEsistenteMenu)
                {
                    Righe = model.Parkings
                    .Where(wh => wh.NomeParcheggio == ParcheggioEsistenteScelto)
                    .Select(s => s.Righe)
                    .FirstOrDefault();


                    Colonne = model.Parkings
                        .Where(wh => wh.NomeParcheggio == ParcheggioEsistenteScelto)
                        .Select(s => s.Colonne)
                        .FirstOrDefault();
                }
                else if (ParcheggioNuovoMenu)
                {
                    Righe = model.Parkings
                        .Where(wh => wh.NomeParcheggio == NomeParcheggioScelto)
                        .Select(s => s.Righe)
                        .FirstOrDefault();


                    Colonne = model.Parkings
                        .Where(wh => wh.NomeParcheggio == NomeParcheggioScelto)
                        .Select(s => s.Colonne)
                        .FirstOrDefault();
                }
            }

            #endregion

            List<string> rigacompleta = new List<string>();
            List<string> colonnacompleta = new List<string>();
            for (int i = 0; i < Convert.ToInt32(Righe); i++)
            {
                for (int g = 0; g < Convert.ToInt32(Colonne); g++)
                {
                    #region Controllo Righe e Colonne

                    if (i < 10 && g >= 10)
                    {
                        rigacompleta.Add("0" + i.ToString());
                        colonnacompleta.Add(g.ToString());
                    }
                    if (i >= 10 && g < 10)
                    {
                        rigacompleta.Add(i.ToString());
                        colonnacompleta.Add("0" + g.ToString());
                    }
                    if (i < 10 && g < 10)
                    {
                        rigacompleta.Add("0" + i.ToString());
                        colonnacompleta.Add("0" + g.ToString());
                    }
                    if (i >= 10 && g >= 10)
                    {
                        rigacompleta.Add(i.ToString());
                        colonnacompleta.Add(g.ToString());
                    }

                    #endregion
                }
            }

            #region Griglia di bottoni

            OttenimentoAutoParcheggiate(status, rigaeliminata, colonnaeliminata);

            grid.Background = new SolidColorBrush(Colors.Gray);
            for (int k = 0; k < Convert.ToInt32(Righe); k++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }
            for (int k = 0; k < Convert.ToInt32(Colonne); k++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(120) });
            }
            for (int k = 0; k < Convert.ToInt32(Righe); k++)
            {
                for (int j = 0; j < Convert.ToInt32(Colonne); j++)
                {
                    Button AreeParcheggio = new Button();
                    AreeParcheggio.Name = "Button" + rigacompleta[k * Convert.ToInt32(Colonne)] + colonnacompleta[j];
                    AreeParcheggio.Margin = new Thickness(2, 2, 0, 0);
                    AreeParcheggio.Background = new SolidColorBrush(Colors.LimeGreen);
                    AreeParcheggio.Click += new RoutedEventHandler(AllButtonClick);
                    foreach (var v in keyValues)
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

            #endregion
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
    }
}