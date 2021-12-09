using Newtonsoft.Json;
using Parcheggio.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logica di interazione per IncassoGiornaliero.xaml
    /// </summary>
    public partial class IncassoAttuale : Window
    {
        public HttpClient client { get; set; } = new HttpClient();
        public string Parcheggio { get; set; }
        public IncassoAttuale(string nomeParcheggio)
        {
            InitializeComponent();
            Parcheggio = nomeParcheggio;
            CompilazioneForm();
            this.DataContext = this;
        }

        public async void CompilazioneForm()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("it-IT");
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:31329/api/IncassoAttuale/{Parcheggio}/{DateTime.Today}")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<List<ParkingAmount>>(await response.Content.ReadAsStringAsync());
            if(data != null)
                Incasso_Giornaliero.ItemsSource = data;
        }
    }
}
