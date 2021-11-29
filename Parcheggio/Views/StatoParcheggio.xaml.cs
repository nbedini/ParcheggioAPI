using Parcheggio.Models;
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
        public StatoParcheggio(object sender, string nomeparcheggio)
        {
            ParcheggioSelezionato = nomeparcheggio;
            CoordinateBottone = ((Button)sender).Name.Substring(6);
            Thread OttenimentoTargaThread = new Thread(new ThreadStart(OttenimentoTarga));
            OttenimentoTargaThread.Start();
            CompilazionePagina(OttenimentoTargaThread);
            InitializeComponent();
            this.DataContext = this;
        }

        public void CompilazionePagina(Thread thread)
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

            thread.Join();

            if(Targa != null)
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

        public void OttenimentoTarga()
        {
            using(ParkingSystemEntities model = new ParkingSystemEntities())
            {
                ParkingStatuss veicolo = model.ParkingStatusses
                    .FirstOrDefault(fod => fod.Riga == this.RigaString && fod.Colonna == this.ColonnaString);
                if( veicolo != null)
                {
                    Targa = veicolo.Targa;
                    TipoVeicolo = veicolo.TipoVeicolo;
                }
            }
        }

        public TimeSpan Esci()
        {
            DateTime OrarioUscita, OrarioIngresso = new DateTime();
            OrarioUscita = DateTime.Now;

            using (ParkingSystemEntities model = new ParkingSystemEntities())
            {
                OrarioIngresso = model.ParkingStatusses
                    .FirstOrDefault(fod => fod.Targa == this.Targa).DataOrarioEntrata;
                TimeSpan TempoTrascorso = OrarioUscita - OrarioIngresso;

                model.ParkingHistorys.Add(new ParkingHistory
                {
                    NomeParcheggio = this.ParcheggioSelezionato,
                    TipoVeicolo = this.TipoVeicolo,
                    TempoTrascorso = TempoTrascorso,
                    Colonna = this.ColonnaString,
                    Riga = this.RigaString,
                    DataOrarioEntrata = OrarioIngresso,
                    DataOrarioUscita = OrarioUscita,
                    Propietario = model.Vehicles
                        .FirstOrDefault(fod => fod.Targa == this.Targa).Propietario,
                    Targa = this.Targa,
                    Tariffa = model.ParkingCosts
                        .FirstOrDefault(fod => fod.TipoVeicolo == this.TipoVeicolo).Tariffa
                });
                model.SaveChanges();

                model.ParkingStatusses
                    .Remove(model.ParkingStatusses
                    .FirstOrDefault(fod => fod.Targa == this.Targa && fod.Riga == this.RigaString && fod.Colonna == this.ColonnaString));
                model.SaveChanges();

                if (model.ParkingAmounts.Where(w => w.NomeParcheggio == this.ParcheggioSelezionato).Count() > 0)
                {
                    var giorno = TimeSpan.FromDays(1) - TimeSpan.FromMilliseconds(1);
                    var candidate = model.ParkingAmounts.FirstOrDefault(fod => fod.NomeParcheggio == this.ParcheggioSelezionato);
                    candidate.IncassoTotale = model.ParkingHistorys
                        .Where(w => w.NomeParcheggio == this.ParcheggioSelezionato && w.TempoTrascorso < giorno)
                        .Sum(s => s.Tariffa)
                        .ToString();
                    model.SaveChanges();
                }
                else
                {
                    var giorno = TimeSpan.FromDays(1) - TimeSpan.FromMilliseconds(1);
                    model.ParkingAmounts.Add(new ParkingAmount
                    {
                        NomeParcheggio = this.ParcheggioSelezionato,
                        IncassoTotale = model.ParkingHistorys
                            .Where(w => w.NomeParcheggio == this.ParcheggioSelezionato && w.TempoTrascorso < giorno)
                            .Sum(s => s.Tariffa)
                            .ToString(),
                        Giorno = DateTime.Today
                    });
                    model.SaveChanges();
                }

                return TempoTrascorso;
            }
        }

        private void ConfermaClick(object sender, RoutedEventArgs e)
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
                    TimeSpan TempoParcheggiato = Esci();
                    MessageBox.Show($"Tempo trascorso: {TempoParcheggiato.ToString().Substring(0, TempoParcheggiato.ToString().IndexOf("."))}", "Tempo trascorso", MessageBoxButton.OK, MessageBoxImage.None);
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
