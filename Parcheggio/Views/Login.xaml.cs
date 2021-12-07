using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ParcheggioAPI.Controllers;
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
        public string UsernameForm { get; set; }
        public string UsernameLogin { get; set; }
        public bool SwitchRegistrazione { get; set; } = false;
        public bool LoginEffettuatoChiusuraForm { get; set; } = false;
        public bool Risposta { get; set; }
        static HttpClient client = new HttpClient();
        public Login()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void SubmitClick(object sender, RoutedEventArgs e)
        {
            string password = tbPassword.Password;
            object candidato = new { username = UsernameForm, password = password };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:31329/api/Login"),
                Content = new StringContent(JsonConvert.SerializeObject(candidato), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<LoginClass>(await response.Content.ReadAsStringAsync());
            UsernameLogin = data.Username;
            if (response.IsSuccessStatusCode)
            {
                Properties.Settings.Token = data.Token;
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.Token);
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
                MessageBox.Show("Password sbagliata");
            }

        }

        private void RegistrazioneClick(object sender, RoutedEventArgs e)
        {
            SwitchRegistrazione = true;
            this.Close();
        }
    }
}
