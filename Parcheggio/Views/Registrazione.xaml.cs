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
using ParcheggioAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Parcheggio.Views
{
    /// <summary>
    /// Logica di interazione per Registrazione.xaml
    /// </summary>
    public partial class Registrazione : Window
    {
        public bool SwitchLogin { get; set; } = false;
        public bool StatusChiusura { get; set; } = false;
        static HttpClient client = new HttpClient();
        public Registrazione()
        {
            InitializeComponent();
        }

        private async void SubmitClick(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Password;
            if (password.Length > 5)
            {


                object candidato = new { username = username, password = password };
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://localhost:31329/api/Crea-Utente"),
                    Content = new StringContent(JsonConvert.SerializeObject(candidato), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                
                if (response.IsSuccessStatusCode)
                {
                    StatusChiusura = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username già utilizzato");
                    StatusChiusura = false;
                }
            }
            else
            {
                MessageBox.Show("Password troppo breve");
                StatusChiusura = false;
            }
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            SwitchLogin = true;
            this.Close();
        }
    }
}
