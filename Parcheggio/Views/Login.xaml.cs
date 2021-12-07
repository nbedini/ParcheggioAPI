using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace Parcheggio.Views
{
    /// <summary>
    /// Logica di interazione per Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public bool SwitchRegistrazione { get; set; } = false;
        public bool LoginEffettuatoChiusuraForm { get; set; } = false;
        public bool Risposta { get; set; }
        static HttpClient client = new HttpClient();
        public Login()
        {
            InitializeComponent();
        }

        private async void SubmitClick(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Password;
            object candidato = new { username = username, password = password };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:31329/api/Login"),
                Content = new StringContent(JsonConvert.SerializeObject(candidato), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                Properties.Settings.Token = data;
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data);
                var request2 = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("http://localhost:31329/api/Admin"),
                    
                };
                var response2 = await client.SendAsync(request2);
                var risposta = await response2.Content.ReadAsStringAsync();
                Risposta = Boolean.Parse(risposta);
                LoginEffettuatoChiusuraForm = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Qualcosa è andato storto");
            }

        }

        private void RegistrazioneClick(object sender, RoutedEventArgs e)
        {
            SwitchRegistrazione = true;
            this.Close();
        }
    }
}
