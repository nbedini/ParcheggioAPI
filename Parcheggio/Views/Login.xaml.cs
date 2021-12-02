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
                MessageBox.Show("Login andato a buon fine");
            }
            else
            {
                MessageBox.Show("Username e password non coincidenti");
            }
        }

        private void RegistrazioneClick(object sender, RoutedEventArgs e)
        {
            Registrazione r = new Registrazione();
            r.Show();
            this.Hide();
        }
    }
}
