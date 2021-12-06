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
using Newtonsoft.Json;
using Parcheggio.Models;

namespace Parcheggio.Views
{
    public partial class ParcheggioEsistente : Window, INotifyPropertyChanged
    {

        #region Properties

        static HttpClient client = new HttpClient();
        public bool ParcheggioSelezionato { get; set; } = false;
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> _parking;
        public List<string> ListaParcheggiEsistenti
        {
            get { return _parking; }
            set 
            { 
                _parking = value;
                OnPropertyChanged("ListaParcheggiEsistenti");
            }
        }

        public string NomeParcheggioSelezionato { get; set; }
        public Brush BackgroundDeleteButton { get; set; } = new SolidColorBrush(Color.FromArgb(255, 210, 1, 1));
        public Brush BackgroundAcceptButton { get; set; } = new SolidColorBrush(Color.FromArgb(255, 0, 165, 35));

        #endregion

        #region Constructor
        public ParcheggioEsistente(string admin)
        {
            GetObtainParkingList();
            InitializeComponent();
            this.DataContext = this;
            if (admin == "false") btElimina.Visibility = System.Windows.Visibility.Collapsed;
            else btElimina.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region Public Methods

        public void OnPropertyChanged(string nome)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
        }

        public async Task GetObtainParkingList()
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:31329/api/existing");
            var stringserialize = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<string>>(stringserialize);
            ListaParcheggiEsistenti = data;
        }

        public async Task DeleteSelectedParking()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri("http://localhost:31329/api/remove"),
                Content = new StringContent(JsonConvert.SerializeObject(NomeParcheggioSelezionato), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
        }

        #endregion

        #region Events

        private void ConfermaParcheggioClick(object sender, RoutedEventArgs e)
        {
            ParcheggioSelezionato = true;
            if (NomeParcheggioSelezionato != null)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Impossibile completare il caricamento non e' stato selezionato nessun parcheggio", "Impossibile caricare il parcheggio", MessageBoxButton.OK, MessageBoxImage.Error);
                ParcheggioSelezionato = false;
            }
        }

        #endregion

        private async void EliminaParcheggioClick(object sender, RoutedEventArgs e)
        {
            if (NomeParcheggioSelezionato != null)
            {
                MessageBoxResult result = MessageBox.Show($"Sei sicuro di voler eliminare il parcheggio che si chiama {NomeParcheggioSelezionato.ToUpper()}","Conferma eliminazione parcheggio", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {
                            await DeleteSelectedParking();
                            await GetObtainParkingList();
                            break;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("Impossibile iniziare l'eliminazione non e' stato selezionato nessun parcheggio", "Impossibile eliminare il parcheggio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
