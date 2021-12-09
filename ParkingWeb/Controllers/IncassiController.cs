using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingWeb.Model;
using ParkingWeb.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using ParkingWeb.ViewModels;

namespace ParkingWeb.Controllers
{
    public class IncassiController : Controller
    {
        [HttpGet("/IncassiStorico/{Name}")]
        public async Task<ActionResult> Index(string Name)
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:31329/api/IncassiStorico/{Name}")
            };
            var response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                IncassiGornalieri incassiGornalieri = new IncassiGornalieri 
                { 
                    Incassi = JsonConvert.DeserializeObject<List<ParkingAmount>>(await response.Content.ReadAsStringAsync()) 
                };
                return View(incassiGornalieri);
            }
            else
                return BadRequest();
        }

        [HttpGet("/IncassiAttuali/{nomeparcheggio}")]
        public async Task<ActionResult> IncassiAttuali(string nomeparcheggio)
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost:31329/api/IncassiAttuali/{nomeparcheggio}")
            };
            var response = await client.SendAsync(request);
            var data = JsonConvert.DeserializeObject<IncassiAttualiViewModel>(await response.Content.ReadAsStringAsync());
            if (data != null)
                return View(data);
            else
                return BadRequest();
        }
    }
}
