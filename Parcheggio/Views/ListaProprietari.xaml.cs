using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logica di interazione per ListaProprietari.xaml
    /// </summary>
    public partial class ListaProprietari : Window
    {
        public string NomeParcheggio { get; set; }
        public ListaProprietari()
        {
            InitializeComponent();
            NomeParcheggio = MainWindow.NomeParcheggioScelto;
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.Source = new Uri($"http://localhost:34483/VisualizzaProprietari/{NomeParcheggio}");
            DefaultGrid.Children.Add(webBrowser);
        }
    }
}
