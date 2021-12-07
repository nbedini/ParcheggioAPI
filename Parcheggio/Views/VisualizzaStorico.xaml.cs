using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Newtonsoft.Json;
using Parcheggio.Models;
using ParcheggioAPI.Models;

namespace Parcheggio.Views
{
    /// <summary>
    /// Logica di interazione per VisualizzaStorico.xaml
    /// </summary>
    public partial class VisualizzaStorico : Window, INotifyPropertyChanged
    {
        HttpClient client = new HttpClient();
        public string Nome { get; set; }
        private List<ParkingHistory> autoparcheggiate;  

        public List<ParkingHistory> AutoParcheggiate
        {
            get { return autoparcheggiate; }
            set { autoparcheggiate = value; }
        }

        private string cercaretarga;

        public string CercareTarga
        {
            get { return cercaretarga; }
            set { cercaretarga = value; }
        }

        private List<ParkingHistory> itemsource;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<ParkingHistory> ItemSource
        {
            get { return itemsource; }
            set 
            { 
                itemsource = value;
                OnPropertyChanged("ItemSource");
            }
        }
        public static ParkingHistory SelectedItem { get; set; }

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public VisualizzaStorico(string nome)
        {
            InitializeComponent();
            Nome = nome;
            GetStorico();
            this.DataContext = this;
        }

        public async Task GetStorico()
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:31329/api/storicoparcheggio/{Nome}")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<List<ParkingHistory>>(await response.Content.ReadAsStringAsync());
            ItemSource = data;
        }
        private async void Proprietario_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedItem != null)
            {
                VisualizzaProprietario visualizzaProprietarioView = new VisualizzaProprietario();
                visualizzaProprietarioView.ShowDialog();
            }
            else
                MessageBox.Show("Non è stata selezionata nessuna riga", "Impossibile visualizzare il proprietario", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public async Task GetFilter(string targaparziale)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:31329/api/storicoricercatarga"),
                Content = new StringContent(JsonConvert.SerializeObject(new RicercaTarga
                {
                    targaparziale = targaparziale,
                    NomeParcheggio = Nome
                }), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<List<ParkingHistory>>(await response.Content.ReadAsStringAsync());
            AutoParcheggiate = data;
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await GetFilter(CercareTarga);
            ItemSource = AutoParcheggiate;
        }
    }
}
