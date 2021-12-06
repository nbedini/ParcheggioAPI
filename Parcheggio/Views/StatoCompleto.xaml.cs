using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Parcheggio.Models;
using Microsoft.SqlServer;
using System.Net.Http;
using Newtonsoft.Json;
using ParcheggioAPI.Models;

namespace Parcheggio.Views
{
    /// <summary>
    /// Logica di interazione per StatoCompleto.xaml
    /// </summary>
    public partial class StatoCompleto : Window, INotifyPropertyChanged
    {
        private string ricercaTarga;

        public string Parcheggio { get; set; }

        HttpClient client = new HttpClient();

        private List<ParkingStatuss> autoparcheggiate;  

        public List<ParkingStatuss> AutoParcheggiate
        {
            get { return autoparcheggiate; }
            set 
            { 
                autoparcheggiate = value;
                OnPropertyChanged("autoparcheggiate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CercareTarga
        {
            get { return this.ricercaTarga; }
            set
            {
                ricercaTarga = value;
                OnPropertyChanged("CercareTarga");                
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StatoCompleto(string nomePark)
        {
            InitializeComponent();
            Parcheggio = nomePark;
            this.DataContext = this;
        }

        public async Task GetStatuss()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:31329/api/statocompletogrid"),
                Content = new StringContent(JsonConvert.SerializeObject(Parcheggio), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<List<ParkingStatuss>>(await response.Content.ReadAsStringAsync());
            AutoParcheggiate = data;
        }

        public async Task GetFilter(string targaparziale)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:31329/api/statocompletoricercatarga"),
                Content = new StringContent(JsonConvert.SerializeObject(new RicercaTarga
                {
                    targaparziale = targaparziale,
                    NomeParcheggio = this.Parcheggio
                }), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<List<ParkingStatuss>>(await response.Content.ReadAsStringAsync());
            AutoParcheggiate = data;
        }

        private void Dettaglio_Click(object sender, RoutedEventArgs e)
        {
            if (this.SC_Parcheggio.SelectedItem != null)
                MessageBox.Show("Dettaglio:" + $"Targa: {((ParkingStatuss)SC_Parcheggio.SelectedItem).Targa}","Dettaglio", MessageBoxButton.OK);
            else
                MessageBox.Show("Impossibile vedere il dettaglio, nessuna riga selezionata", "Impossibile", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void Reset(object sender, RoutedEventArgs e)
        {
            await GetStatuss();
            SC_Parcheggio.ItemsSource = AutoParcheggiate;
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await GetFilter(CercareTarga);
            SC_Parcheggio.ItemsSource = AutoParcheggiate;
        }

        private async void FormLoaded(object sender, RoutedEventArgs e)
        {
            await GetStatuss();
            SC_Parcheggio.ItemsSource = AutoParcheggiate;
        }
    }
}
