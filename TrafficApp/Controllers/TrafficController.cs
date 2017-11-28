using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrafficApp.Models;
using TrafficApp.Integration;
using JSM;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TrafficApp.Controllers
{
    public class TrafficController : Controller
    {
        public IActionResult Home()
        {
            byte[] apiKeyArray;
            HttpContext.Session.TryGetValue("apiKey", out apiKeyArray);
            ViewData["apiKey"] = Encoding.ASCII.GetString(apiKeyArray);

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                          .AddJsonFile("appsettings.json").Build();
            ViewData["baseUrl"] = configuration["BaseUrl"];

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FindRouteAction(CalculationInputModel model)
        {
            var startAddress = model.StartAddress;
            var destinationAddress = model.DestinationAddress;
            /*
            Tuple<double, double> StartCoordinates = nominatimService.GetCoordinates(startAddress).Result;
            Tuple<double, double> DestinationCoordinates = nominatimService.GetCoordinates(destinationAddress).Result;

            if (StartCoordinates != null || destinationAddress != null) {

                var trafficService = new TrafficCalculationService();
                // Test: Hardcoded coordinates
                double startLat = 54.7808636;
                double startLon = 11.4887596;
                double destLat = 54.7788018;
                double destLon = 11.4809834;
                Route route = trafficService.CalculateRoute(startLat, startLon, destLat, destLon, DateTime.Now, "abcdefg").Result;
            */
            /*
            } else {
                // TODO: View Error message
            }
            */

            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
