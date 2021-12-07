using Parcheggio.Models;
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
    /// Logica di interazione per VisualizzaProprietario.xaml
    /// </summary>
    public partial class VisualizzaProprietario : Window
    {
        public ParkingHistory SelectedItem { get; set; }
        public VisualizzaProprietario()
        {
            InitializeComponent();
            SelectedItem = VisualizzaStorico.SelectedItem;
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.Source = new Uri($"http://localhost:34483/VisualizzaProprietario/{SelectedItem.Propietario}/{SelectedItem.NomeParcheggio}");
            GridDefault.Children.Add(webBrowser);
            this.DataContext = this;
        }
    }
}
