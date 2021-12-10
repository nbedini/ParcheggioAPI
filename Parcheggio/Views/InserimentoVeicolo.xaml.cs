using Newtonsoft.Json;
using Parcheggio.Models;
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

        HttpClient client = new HttpClient();


        public InserimentoVeicolo(string riga,string colonna, string nomeparcheggio, string targa = "")
        {
            DatiDB();
            RigaSelezionata = riga;
            ColonnaSelezionata = colonna;
            NomeParcheggio = nomeparcheggio;
            Targa = targa;
            InitializeComponent();
            this.DataContext = this;
        }
        public InserimentoVeicolo(string riga, string colonna, string nomeparcheggio, Person propietario, Vehicle veicolo)
        {
            DatiDB();
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
                        TipoVeicolo = "Automobile";
                        break;
                    }
                case "Camion":
                    {
                        TipoVeicolo = "Camion";
                        break;
                    }
                case "Moto":
                    {
                        TipoVeicolo = "Moto";
                        break;
                    }
            }
            Targa = veicolo.Targa;
            InitializeComponent();
            this.DataContext = this;
        }

        public void DatiDB()
        {
            Thread thread = new Thread(new ThreadStart(RecuperoDatiDB));
            thread.Start();
            thread.Join();
        }

        public async void RecuperoDatiDB()
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:31329/api/datiDB")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<DatiInserimentoVeicolo>(await response.Content.ReadAsStringAsync());
            TipiVeicoli = data.TipiVeicoli;
            ProprietariAttualmenteRegistrati = data.ProprietariAttualmenteRegistrati;
            VeicoliAttualmenteParcheggiati = data.VeicoliAttualmenteParcheggiati;
            return;
        }

        public async Task InserimentoDatiDB()
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://localhost:31329/api/inserimentoveicolo"),
                Content = new StringContent(JsonConvert.SerializeObject(new InserimentoVeicoloConProprietario
                {
                    Person = new Person
                    {
                        CodiceFiscale = this.CodiceFiscale,
                        Nome = this.Nome,
                        Cognome = this.Cognome,
                        DataNascita = this.DataNascita
                    },
                    Veicolo = new Vehicle
                    {
                        Marca = this.Marca,
                        Modello = this.Modello,
                        Targa = this.Targa,
                        Propietario = this.CodiceFiscale,
                        TipoVeicolo = this.TipoVeicolo
                    },
                    TabellaInserimento = new ParkingStatuss
                    {
                        Targa = this.Targa,
                        Riga = RigaSelezionata,
                        Colonna = ColonnaSelezionata,
                        DataOrarioEntrata = DateTime.Now,
                        NomeParcheggio = this.NomeParcheggio,
                        TipoVeicolo = this.TipoVeicolo
                    }
                }), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return;
        }

        public async Task InserimentoDatiDBConProprietarioEsistente()
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://localhost:31329/api/inserimentoveicolosenzaproprietario"),
                Content = new StringContent(JsonConvert.SerializeObject(new InserimentoVeicoloSenzaProprietario
                {
                    Veicolo = new Vehicle
                    {
                        Marca = this.Marca,
                        Modello = this.Modello,
                        Targa = this.Targa,
                        Propietario = this.CodiceFiscale,
                        TipoVeicolo = this.TipoVeicolo
                    },
                    TabellaInserimento = new ParkingStatuss
                    {
                        Targa = this.Targa,
                        Riga = RigaSelezionata,
                        Colonna = ColonnaSelezionata,
                        DataOrarioEntrata = DateTime.Now,
                        NomeParcheggio = this.NomeParcheggio,
                        TipoVeicolo = this.TipoVeicolo
                    }
                }), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return;
        }

        private async void ConfermaClick(object sender, RoutedEventArgs e)
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
                        await InserimentoDatiDBConProprietarioEsistente();
                        this.Close();
                        return;
                    }
                }
                await InserimentoDatiDB();
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
