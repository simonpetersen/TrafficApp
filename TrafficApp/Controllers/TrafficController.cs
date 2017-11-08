using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrafficApp.Models;
using TrafficApp.Integration;
namespace TrafficApp.Controllers
{
    public class TrafficController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FindRouteAction(CalculationInputModel model)
        {
            //var calculationService = new TrafficCalculationService();
            var nominatimService = new NominatimGeocodingService();
            var startAddress = model.StartAddress;
            var destinationAddress = model.DestinationAddress;

            Tuple<double, double> StartCoordinates = nominatimService.GetCoordinates(startAddress).Result;
            Tuple<double, double> DestinationCoordinates = nominatimService.GetCoordinates(destinationAddress).Result;

            if (StartCoordinates != null || destinationAddress != null) {
                // TODO: Call TrafficCalculationServive()
            } else {
                // TODO: View Error message
            }

            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
