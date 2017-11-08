using System;
using Microsoft.AspNetCore.Mvc;
using TrafficApp.Integration;
using TrafficApp.Models;

namespace TrafficApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login() 
        {
            return View();
        }

        public IActionResult LoginAction(LoginModel model)
        {
            //var calculationService = new TrafficCalculationService();
            //var user = calculationService.Login(model.Username, model.Password);

            if (model.Username != null && model.Username.Equals("admin") && model.Password != null && model.Password.Equals("1234")) {
                return RedirectToAction("Home", "Traffic");
            }

            return View("Login");
        }
    }
}
