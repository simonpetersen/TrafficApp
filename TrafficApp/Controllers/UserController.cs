using System;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrafficApp.Integration;
using TrafficApp.Models;
using System.Text;

namespace TrafficApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login() 
        {
            return View();
        }

        public IActionResult Admin() 
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(UserModel model)
        {
            var calculationService = new TrafficCalculationService();
            var createModel = model.CreateModel;
            var adminString = createModel.Admin ? "true" : "false";
            var apiKey = GetApiKey();
            var result = await calculationService.CreateUser(createModel.Username, createModel.Password, createModel.Name, adminString, apiKey);

            return View("Admin", new UserModel() { CreateMessage = result });
        }

        public async Task<IActionResult> DeleteUser(UserModel model)
        {
            var calculationService = new TrafficCalculationService();
            var deleteModel = model.DeleteModel;
            var apiKey = GetApiKey();
            var result = await calculationService.DeleteUser(deleteModel.Username, apiKey);

            return View("Admin", new UserModel() { DeleteMessage = result });
        }

        public async Task<IActionResult> LoginAction(LoginModel model)
        {
            /*
            var calculationService = new TrafficCalculationService();
            var user = await calculationService.Login(model.Username, model.Password);

            if (user != null) {
                TempData["apiKey"] = user.apiKey;
                return RedirectToAction("Home", "Traffic");
            }
            */

            if (model.Username != null)
            {
                //TempData["apiKey"] = "abcdefg";
                HttpContext.Session.Set("apiKey", Encoding.ASCII.GetBytes("abcdefg"));
                return RedirectToAction("Home", "Traffic");
            }

            return View("Login");
        }

        private string GetApiKey()
        {
            byte[] apiKeyArray;
            HttpContext.Session.TryGetValue("apiKey", out apiKeyArray);
            return Encoding.ASCII.GetString(apiKeyArray);
        }
    }
}
