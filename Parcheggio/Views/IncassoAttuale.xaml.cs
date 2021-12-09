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
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.Source = new Uri($"http://localhost:34483/IncassiAttuali/{Parcheggio}");
            DefaultGrid.Children.Add(webBrowser);
            this.DataContext = this;
        }
    }
}
