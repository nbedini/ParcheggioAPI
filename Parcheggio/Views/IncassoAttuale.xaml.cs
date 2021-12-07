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
    /// Logica di interazione per IncassoGiornaliero.xaml
    /// </summary>
    public partial class IncassoAttuale : Window
    {
        public string Parcheggio { get; set; }
        public IncassoAttuale(string nomeParcheggio)
        {
            InitializeComponent();

            Parcheggio = nomeParcheggio;

            this.DataContext = this;
        }
    }
}
