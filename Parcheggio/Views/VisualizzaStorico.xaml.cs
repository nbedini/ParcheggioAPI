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
using Parcheggio.Models;

namespace Parcheggio.Views
{
    /// <summary>
    /// Logica di interazione per VisualizzaStorico.xaml
    /// </summary>
    public partial class VisualizzaStorico : Window
    {
        public string Nome { get; set; }
        public VisualizzaStorico(string nome)
        {
            InitializeComponent();
            Nome = nome;
            using(ParkingSystemEntities model = new ParkingSystemEntities())
            {
                Storico_Parcheggio.ItemsSource = model.ParkingHistorys.Where(o => o.NomeParcheggio == Nome).ToList();
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            using (ParkingSystemEntities model = new ParkingSystemEntities())
            {
                Storico_Parcheggio.ItemsSource = model.ParkingHistorys.Where(o => o.NomeParcheggio == Nome).ToList();
            }
        }
        private void Proprietario_Click(object sender, RoutedEventArgs e)
        {
            using (ParkingSystemEntities model = new ParkingSystemEntities())
            {
                var veicoloS = Storico_Parcheggio.SelectedItem;
                
            }
        }
        private void Cerca_Click(object sender, RoutedEventArgs e)
        {
            using (ParkingSystemEntities model = new ParkingSystemEntities())
            {
                Storico_Parcheggio.ItemsSource = model.ParkingHistorys.Where(o => o.Targa.Equals(tb_RicercaTarga.Text.ToString())).ToList();
            }
        }

    }
}
