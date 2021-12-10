using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ParcheggioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParcheggioAPI.Controllers
{
    
    [ApiController]
    public class ParcheggioViewController : ControllerBase
    {
        public Logger logger { get; set; } = LogManager.GetCurrentClassLogger();
        [HttpPost("/api/parcheggioview")]
        public ValoreRitornoParcheggioView ParcheggioView([FromBody]DatiParcheggio datiparcheggio)
        {
            var Righe = "";
            var Colonne = "";

            if (datiparcheggio.NomeParcheggio == null)
                return null;
            else
            {

                #region Ottenimento righe e colonne dal database

                using (ParkingSystemContext model = new ParkingSystemContext())
                {
                    if (datiparcheggio.ParcheggioEsistenteMenu)
                    {
                        Righe = model.Parkings
                        .Where(wh => wh.NomeParcheggio == datiparcheggio.NomeParcheggio)
                        .Select(s => s.Righe)
                        .FirstOrDefault();


                        Colonne = model.Parkings
                            .Where(wh => wh.NomeParcheggio == datiparcheggio.NomeParcheggio)
                            .Select(s => s.Colonne)
                            .FirstOrDefault();
                    }
                    else if (datiparcheggio.ParcheggioNuovoMenu)
                    {
                        Righe = model.Parkings
                            .Where(wh => wh.NomeParcheggio == datiparcheggio.NomeParcheggio)
                            .Select(s => s.Righe)
                            .FirstOrDefault();


                        Colonne = model.Parkings
                            .Where(wh => wh.NomeParcheggio == datiparcheggio.NomeParcheggio)
                            .Select(s => s.Colonne)
                            .FirstOrDefault();
                    }
                }

                #endregion

                List<string> rigacompleta = new List<string>();
                List<string> colonnacompleta = new List<string>();
                for (int i = 0; i < Convert.ToInt32(Righe); i++)
                {
                    for (int g = 0; g < Convert.ToInt32(Colonne); g++)
                    {
                        #region Controllo Righe e Colonne

                        if (i < 10 && g >= 10)
                        {
                            rigacompleta.Add("0" + i.ToString());
                            colonnacompleta.Add(g.ToString());
                        }
                        if (i >= 10 && g < 10)
                        {
                            rigacompleta.Add(i.ToString());
                            colonnacompleta.Add("0" + g.ToString());
                        }
                        if (i < 10 && g < 10)
                        {
                            rigacompleta.Add("0" + i.ToString());
                            colonnacompleta.Add("0" + g.ToString());
                        }
                        if (i >= 10 && g >= 10)
                        {
                            rigacompleta.Add(i.ToString());
                            colonnacompleta.Add(g.ToString());
                        }

                        #endregion
                    }
                }

                ValoreRitornoParcheggioView ParcheggioView = new ValoreRitornoParcheggioView
                {
                    Righe = Righe,
                    Colonne = Colonne,
                    rigacompleta = rigacompleta,
                    colonnacompleta = colonnacompleta,
                    keyValues = MetodiSupporto.keyValues
                };

                MetodiSupporto.AutoParcheggiate(datiparcheggio);

                return ParcheggioView;
            }
        }
    }
}
