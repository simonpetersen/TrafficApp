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
            return byteArray != null && Encoding.ASCII.GetString(byteArray).Equals(true.ToString());
        }
    }
}
