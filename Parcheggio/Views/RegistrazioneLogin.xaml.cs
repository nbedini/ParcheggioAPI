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
    /// Logica di interazione per RegistrazioneLogin.xaml
    /// </summary>
    public partial class RegistrazioneLogin : Window
    {
        public bool AdminONo { get; set; }
        public RegistrazioneLogin()
        {
            InitializeComponent();
        }

        private void RegistrazioneClick(object sender, RoutedEventArgs e)
        {
            Registrazione r = new Registrazione();
            r.ShowDialog();
            AdminONo = r.StatusChiusura;
            this.Close();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            l.ShowDialog();
            AdminONo = l.Risposta;
            this.Close();
        }
    }
}
