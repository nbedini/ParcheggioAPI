using Parcheggio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logica di interazione per InserimentoVeicolo.xaml
    /// </summary>
    public partial class InserimentoVeicolo : Window, INotifyPropertyChanged
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public DateTime DataNascita { get; set; } = DateTime.Now.Date;
        public string CodiceFiscale { get; set; }
        public string Marca { get; set; }
        public string Modello { get; set; }
        public string Targa { get; set; }
        public string RigaSelezionata { get; set; }
        public string ColonnaSelezionata { get; set; }
        public string NomeParcheggio { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private string _tipoveicolo;

        public string TipoVeicolo
        {
            get { return _tipoveicolo; }
            set 
            { 
                _tipoveicolo = value;
                OnPropertyChanged("TipoVeicolo");
            }
        }

        public List<string> TipiVeicoli { get; set; } = new List<string>();
        public List<ParkingStatuss> VeicoliAttualmenteParcheggiati { get; set; }
        public List<Person> ProprietariAttualmenteRegistrati { get; set; }


        public InserimentoVeicolo(string riga,string colonna, string nomeparcheggio, string targa = "")
        {
            Thread thread = new Thread(new ThreadStart(RecuperoDatiDB));
            thread.Start();
            RigaSelezionata = riga;
            ColonnaSelezionata = colonna;
            NomeParcheggio = nomeparcheggio;
            Targa = targa;
            InitializeComponent();
            this.DataContext = this;
        }
        public InserimentoVeicolo(string riga, string colonna, string nomeparcheggio, Person propietario, Vehicle veicolo)
        {
            Thread thread = new Thread(new ThreadStart(RecuperoDatiDB));
            thread.Start();
            RigaSelezionata = riga;
            ColonnaSelezionata = colonna;
            NomeParcheggio = nomeparcheggio;
            Nome = propietario.Nome;
            Cognome = propietario.Cognome;
            DataNascita = propietario.DataNascita;
            CodiceFiscale = propietario.CodiceFiscale;
            Marca = veicolo.Marca;
            Modello = veicolo.Modello;
            switch (veicolo.TipoVeicolo)
            {
                case "Automobile":
                    {
                        thread.Join();
                        TipoVeicolo = TipiVeicoli[0];
                        break;
                    }
                case "Camion":
                    {
                        thread.Join();
                        TipoVeicolo = TipiVeicoli[1];
                        break;
                    }
                case "Moto":
                    {
                        thread.Join();
                        TipoVeicolo = TipiVeicoli[2];
                        break;
                    }
            }
            Targa = veicolo.Targa;
            InitializeComponent();
            this.DataContext = this;
        }

        public void RecuperoDatiDB()
        {
            using(ParkingSystemEntities model = new ParkingSystemEntities())
            {
                VeicoliAttualmenteParcheggiati = model.ParkingStatusses
                    .ToList();

                ProprietariAttualmenteRegistrati = model.Persons
                    .ToList();

                foreach (var v in model.ParkingCosts)
                {
                    TipiVeicoli.Add(v.TipoVeicolo);
                }
            }
        }

        public void InserimentoDatiDB()
        {
            using (ParkingSystemEntities model = new ParkingSystemEntities())
            {
                Person person = new Person { Nome = this.Nome, Cognome = this.Cognome, DataNascita = this.DataNascita, CodiceFiscale = this.CodiceFiscale };

                model.Persons
                    .Add(person);
                model.Vehicles
                    .Add(new Vehicle { Marca = this.Marca, Modello = this.Modello, Targa = this.Targa, Propietario = this.CodiceFiscale, TipoVeicolo = this.TipoVeicolo });
                model.ParkingStatusses
                        .Add(new ParkingStatuss { Targa = this.Targa, Riga = this.RigaSelezionata, Colonna = this.ColonnaSelezionata, DataOrarioEntrata = DateTime.Now, NomeParcheggio = this.NomeParcheggio, TipoVeicolo = this.TipoVeicolo });
                model.SaveChanges();
            }
        }

        public void InserimentoDatiDBConProprietarioEsistente()
        {
            using (ParkingSystemEntities model = new ParkingSystemEntities())
            {
                if (model.Vehicles.Where(w => w.Targa == this.Targa).Count() == 0)
                {
                    model.Vehicles
                        .Add(new Vehicle { Marca = this.Marca, Modello = this.Modello, Targa = this.Targa, Propietario = this.CodiceFiscale, TipoVeicolo = this.TipoVeicolo });
                    model.ParkingStatusses
                        .Add(new ParkingStatuss { Targa = this.Targa, Riga = this.RigaSelezionata, Colonna = this.ColonnaSelezionata, DataOrarioEntrata = DateTime.Now, NomeParcheggio = this.NomeParcheggio, TipoVeicolo = this.TipoVeicolo });
                    model.SaveChanges();
                }
                else
                {
                    model.ParkingStatusses
                        .Add(new ParkingStatuss { Targa = this.Targa, Riga = this.RigaSelezionata, Colonna = this.ColonnaSelezionata, DataOrarioEntrata = DateTime.Now, NomeParcheggio = this.NomeParcheggio, TipoVeicolo = this.TipoVeicolo });
                    model.SaveChanges();
                }
            }
        }

        private void ConfermaClick(object sender, RoutedEventArgs e)
        {
            if (Nome != null && Cognome != null && CodiceFiscale != null && Marca != null && Modello != null && Targa != null)
            {
                foreach (ParkingStatuss ps in VeicoliAttualmenteParcheggiati)
                {

                    if (ps.Targa == Targa)
                    {
                        MessageBox.Show("Impossibile inserire piu' volte la stessa targa", "Inserimento Impossibile", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                foreach (Person p in ProprietariAttualmenteRegistrati)
                {

                    if (p.CodiceFiscale == CodiceFiscale)
                    {
                        InserimentoDatiDBConProprietarioEsistente();
                        this.Close();
                        return;
                    }
                }
                InserimentoDatiDB();
                this.Close();
            }
            else
            {
                MessageBox.Show("Non sono stati inseriti i dati in modo corretto", "Inserimento Impossibile", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
