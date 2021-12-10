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
    /// Logica di interazione per ControlloTarga.xaml
    /// </summary>
    public partial class ControlloTarga : Window
    {
        public bool ChiusuraconBottone { get; set; } = false;
        public string Targa { get; set; }
        public string NomeParcheggio { get; set; }
        public string Riga { get; set; }
        public string Colonna { get; set; }
        public Person Propietario { get; set; }
        public Vehicle Veicolo { get; set; }
        public ControlloTarga(string riga, string colonna, string nomeparcheggio)
        {
            Riga = riga;
            Colonna = colonna;
            NomeParcheggio = nomeparcheggio;
            InitializeComponent();
            this.DataContext = this;
        }

        private void AccettaClick(object sender, RoutedEventArgs e)
        {
            ChiusuraconBottone = true;
            string CodiceFiscale = "";
            using(ParkingSystemEntities model = new ParkingSystemEntities())
            {        
                if(model.Vehicles.FirstOrDefault(fod => fod.Targa == Targa) != null)
                {
                    CodiceFiscale = model.Vehicles.FirstOrDefault(fod => fod.Targa == Targa).Propietario;
                    Propietario = model.Persons
                        .FirstOrDefault(fod => fod.CodiceFiscale == CodiceFiscale);
                    Veicolo = model.Vehicles
                        .FirstOrDefault(fod => fod.Targa == Targa);
                    InserimentoVeicolo inserimentoVeicoloView = new InserimentoVeicolo(Riga, Colonna, NomeParcheggio, Propietario, Veicolo);
                    inserimentoVeicoloView.ShowDialog();
                    this.Close();
                }
                else
                {
                    InserimentoVeicolo inserimentoVeicoloView = new InserimentoVeicolo(Riga, Colonna, NomeParcheggio, Targa);
                    inserimentoVeicoloView.ShowDialog();
                    this.Close();
                }
            }
        }
    }
}
