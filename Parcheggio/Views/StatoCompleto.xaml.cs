using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.SqlServer;

namespace Parcheggio.Views
{
    /// <summary>
    /// Logica di interazione per StatoCompleto.xaml
    /// </summary>
    public partial class StatoCompleto : Window, INotifyPropertyChanged
    {
        private string ricercaTarga;

        public string Parcheggio { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CercareTarga
        {
            get { return this.ricercaTarga; }
            set
            {
                ricercaTarga = value;
                OnPropertyChanged("CercareTarga");                
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StatoCompleto(string nomePark)
        {
            InitializeComponent();
            Parcheggio = nomePark;

            using(ParkingSystemEntities model = new ParkingSystemEntities())
            {
                SC_Parcheggio.ItemsSource = model.ParkingStatusses.Where(o=>o.NomeParcheggio == Parcheggio).ToList();
            }
            this.DataContext = this;
        }

        private void Dettaglio_Click(object sender, RoutedEventArgs e)
        {
            if (this.SC_Parcheggio.SelectedItem != null)
                MessageBox.Show("Dettaglio:" + $"Targa: {((ParkingStatuss)SC_Parcheggio.SelectedItem).Targa}","Dettaglio", MessageBoxButton.OK);
            else
                MessageBox.Show("Impossibile vedere il dettaglio, nessuna riga selezionata", "Impossibile", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
           
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            using(ParkingSystemEntities model = new ParkingSystemEntities())
            {
                List<ParkingStatuss> filtro = model.ParkingStatusses.Where(o => o.Targa.Contains(CercareTarga) && o.NomeParcheggio.Equals(Parcheggio)).ToList();
                //al momento di ricerca scompare tutto da fixare
            }
        }
    }
}
