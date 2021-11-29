using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace Parcheggio.Views
{
    /// <summary>
    /// Logica di interazione per NuovoParcheggio.xaml
    /// </summary>
    public partial class NuovoParcheggio : Window
    {
        #region Properties

        static HttpClient client = new HttpClient();
        public string Righe { get; set; }
        public string Colonne { get; set; }
        public string NomeParcheggio { get; set; }
        public bool ParcheggioCreato { get; set; } = false;

        #endregion

        #region Costructor
        public NuovoParcheggio()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("http://localhost:31329/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            this.DataContext = this;
        }

        #endregion

        #region Events

        private void ConfermaCreaParcheggioClick(object sender, RoutedEventArgs e)
        {

            //ParcheggioCreato = true;
            //if(Righe != null && Colonne != null && NomeParcheggio != null)
            //{
            //    using(ParkingSystemEntities model = new ParkingSystemEntities())
            //    {

            //        List<Parking> ListaParcheggi = model.Parkings
            //            .ToList();

            //        foreach(var v in ListaParcheggi)
            //        {
            //            if (v.NomeParcheggio == NomeParcheggio)
            //            {
            //                MessageBox.Show("Impossibile creare il parcheggio perche' esiste gia' un parcheggio con lo stesso nome", "Creazione annullata", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }
            //        }

            //        model.Parkings
            //            .Add(new Parking { Colonne = Colonne, NomeParcheggio = NomeParcheggio, Righe = Righe });
            //        model.SaveChanges();
            //        this.Close();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Impossibile creare il parcheggio, non sono stati inseriti tutti i campi necessari", "Creazione Impossibile", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            PostInsertNewParking();

        }

        public async Task PostInsertNewParking()
        {
            HttpResponseMessage response = await client.PostAsync("http://localhost:31329/api/parking/create", new StringContent( JsonConvert.SerializeObject(new { Righe = this.Righe, Colonne = this.Colonne, NomeParcheggio = this.NomeParcheggio }), Encoding.UTF8, "application/json"));
            if(response.IsSuccessStatusCode)
            {
                MessageBox.Show("Creazione andata a buon fine");
            }
        }

        #endregion
    }
}
