using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrafficApp.Models;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TrafficApp.Controllers
{
    public class TrafficController : Controller
    {
        public IActionResult Home()
        {
            var apiKey = GetApiKey();

            if (apiKey == null) 
            {
                return RedirectToAction("Login", "User");
            }

            ViewData["apiKey"] = apiKey;
            ViewData["admin"] = IsAdmin();

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            ViewData["baseUrl"] = configuration["BaseUrl"];

            return View();
        }

        public IActionResult About()
        {
            ViewData["admin"] = IsAdmin();

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

        string GetApiKey()
        {
            byte[] apiKeyArray;
            HttpContext.Session.TryGetValue("apiKey", out apiKeyArray);
            return apiKeyArray != null ? Encoding.ASCII.GetString(apiKeyArray) : null;
        }

        bool IsAdmin() {
            byte[] byteArray;
            HttpContext.Session.TryGetValue("admin", out byteArray);
            return byteArray != null ? true : false;
        }
    }
}
