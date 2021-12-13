using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Parcheggio.Models;
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
        public bool ChiusuraSenzaSelezione { get; set; } = true;
        public bool LoginCompletato { get; set; } = false;
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
            ChiusuraSenzaSelezione = false;
            string password = tbPassword.Password;
            object candidato = new User { Username = UsernameForm, Password = password};
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:31329/api/Login"),
                Content = new StringContent(JsonConvert.SerializeObject(candidato), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            
            
            if (response.IsSuccessStatusCode)
            {

                var data = JsonConvert.DeserializeObject<LoginClass>(await response.Content.ReadAsStringAsync());
                Properties.Settings.Username = data.Username;
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
                LoginEffettuatoChiusuraForm = true;//segna che il login è avvenuto correttamente
                LoginCompletato = true;
                UsernameLogin = Properties.Settings.Username;
                this.Close();
            }
            else
            {
                Properties.Settings.Username = "";
                Properties.Settings.Token = "";
                MessageBox.Show("Username o password sbagliata");
            }

        }

        private void RegistrazioneClick(object sender, RoutedEventArgs e)
        {
            ChiusuraSenzaSelezione = false;
            SwitchRegistrazione = true;
            this.Close();
        }
    }
}
