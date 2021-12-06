using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParcheggioAPI
{
    public class MetodiSupporto
    {

        public static Dictionary<string, string> keyValues = new Dictionary<string, string>();

        public static List<ParkingStatuss> AutoParcheggiateDB(string nomeparcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                return model.ParkingStatusses
                            .Where(w => w.NomeParcheggio == nomeparcheggio)
                            .ToList();
            }
        }

        public static void AutoParcheggiate(DatiParcheggio datiParcheggio)
        {
            var VeicoliParcheggiati = AutoParcheggiateDB(datiParcheggio.NomeParcheggio);

            string riga = "", colonna = "";
            if (datiParcheggio != null)
            {
                if (datiParcheggio.Status == 1)
                {
                    #region Controllo Righe e Colonne

                    foreach (var v in VeicoliParcheggiati)
                    {
                        if (Convert.ToInt32(v.Riga) < 10 && Convert.ToInt32(v.Colonna) >= 10)
                        {
                            riga = "0" + v.Riga;
                            colonna = v.Colonna;
                        }
                        if (Convert.ToInt32(v.Riga) >= 10 && Convert.ToInt32(v.Colonna) < 10)
                        {
                            riga = v.Riga;
                            colonna = "0" + v.Colonna;
                        }
                        if (Convert.ToInt32(v.Riga) < 10 && Convert.ToInt32(v.Colonna) < 10)
                        {
                            riga = "0" + v.Riga;
                            colonna = "0" + v.Colonna;
                        }
                        if (Convert.ToInt32(v.Riga) >= 10 && Convert.ToInt32(v.Colonna) >= 10)
                        {
                            riga = v.Riga;
                            colonna = v.Colonna;
                        }
                        keyValues.Add(riga + colonna, v.Targa);
                    }

                    #endregion

                }
                else if (datiParcheggio.Status == 2)
                {
                    #region Controllo Righe e Colonne

                    if (Convert.ToInt32(datiParcheggio.rigaeliminata) < 10 && Convert.ToInt32(datiParcheggio.colonnaeliminata) >= 10)
                    {
                        riga = "0" + datiParcheggio.rigaeliminata;
                        colonna = datiParcheggio.colonnaeliminata;
                    }
                    if (Convert.ToInt32(datiParcheggio.rigaeliminata) >= 10 && Convert.ToInt32(datiParcheggio.colonnaeliminata) < 10)
                    {
                        riga = datiParcheggio.rigaeliminata;
                        colonna = "0" + datiParcheggio.colonnaeliminata;
                    }
                    if (Convert.ToInt32(datiParcheggio.rigaeliminata) < 10 && Convert.ToInt32(datiParcheggio.colonnaeliminata) < 10)
                    {
                        riga = "0" + datiParcheggio.rigaeliminata;
                        colonna = "0" + datiParcheggio.colonnaeliminata;
                    }
                    if (Convert.ToInt32(datiParcheggio.rigaeliminata) >= 10 && Convert.ToInt32(datiParcheggio.colonnaeliminata) >= 10)
                    {
                        riga = datiParcheggio.rigaeliminata;
                        colonna = datiParcheggio.colonnaeliminata;
                    }

                    #endregion

                    keyValues.Remove(riga + colonna);

                }
                else if (datiParcheggio.Status == 3)
                {
                    #region Controllo Righe e Colonne

                    if (Convert.ToInt32(datiParcheggio.rigaeliminata) < 10 && Convert.ToInt32(datiParcheggio.colonnaeliminata) >= 10)
                    {
                        riga = "0" + datiParcheggio.rigaeliminata;
                        colonna = datiParcheggio.colonnaeliminata;
                    }
                    if (Convert.ToInt32(datiParcheggio.rigaeliminata) >= 10 && Convert.ToInt32(datiParcheggio.colonnaeliminata) < 10)
                    {
                        riga = datiParcheggio.rigaeliminata;
                        colonna = "0" + datiParcheggio.colonnaeliminata;
                    }
                    if (Convert.ToInt32(datiParcheggio.rigaeliminata) < 10 && Convert.ToInt32(datiParcheggio.colonnaeliminata) < 10)
                    {
                        riga = "0" + datiParcheggio.rigaeliminata;
                        colonna = "0" + datiParcheggio.colonnaeliminata;
                    }
                    if (Convert.ToInt32(datiParcheggio.rigaeliminata) >= 10 && Convert.ToInt32(datiParcheggio.colonnaeliminata) >= 10)
                    {
                        riga = datiParcheggio.rigaeliminata;
                        colonna = datiParcheggio.colonnaeliminata;
                    }

                    #endregion

                    using (ParkingSystemContext model = new ParkingSystemContext())
                    {
                        if (model.ParkingStatusses.FirstOrDefault(fod => fod.Riga == datiParcheggio.rigaeliminata && fod.Colonna == datiParcheggio.colonnaeliminata) != null)
                        {
                            string targa = model.ParkingStatusses.FirstOrDefault(fod => fod.Riga == datiParcheggio.rigaeliminata && fod.Colonna == datiParcheggio.colonnaeliminata).Targa;
                            keyValues.Add(riga + colonna, targa);
                        }
                    }

                }
                else if (datiParcheggio.Status == 4)
                {
                    keyValues = new Dictionary<string, string>();

                    #region Controllo Righe e Colonne

                    foreach (var v in VeicoliParcheggiati)
                    {
                        if (Convert.ToInt32(v.Riga) < 10 && Convert.ToInt32(v.Colonna) >= 10)
                        {
                            riga = "0" + v.Riga;
                            colonna = v.Colonna;
                        }
                        if (Convert.ToInt32(v.Riga) >= 10 && Convert.ToInt32(v.Colonna) < 10)
                        {
                            riga = v.Riga;
                            colonna = "0" + v.Colonna;
                        }
                        if (Convert.ToInt32(v.Riga) < 10 && Convert.ToInt32(v.Colonna) < 10)
                        {
                            riga = "0" + v.Riga;
                            colonna = "0" + v.Colonna;
                        }
                        if (Convert.ToInt32(v.Riga) >= 10 && Convert.ToInt32(v.Colonna) >= 10)
                        {
                            riga = v.Riga;
                            colonna = v.Colonna;
                        }
                        keyValues.Add(riga + colonna, v.Targa);
                    }

                    #endregion

                }
            }
        }

        public static Vehicle OttieniVeicolo(string targa)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                Vehicle veicolo = model.Vehicles
                    .FirstOrDefault(o=>o.Targa==targa);
                if (veicolo != null)
                {
                    return veicolo;
                }
                else
                    return null;
            }
        }

        public static string OttenimentoTarga(string riga,string colonna,string nomeParcheggio)
        {
            using (ParkingSystemContext model = new ParkingSystemContext())
            {
                ParkingStatuss veicolo = model.ParkingStatusses
                    .FirstOrDefault(fod => fod.Riga == riga && fod.Colonna == colonna && fod.NomeParcheggio == nomeParcheggio);
                if (veicolo != null)
                {
                    return veicolo.Targa;
                }
                else
                    return null;
            }
        }
    }
}
