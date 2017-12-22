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
            var admin = IsAdmin();
            if (!admin)
            {
                return View("Login");
            }

            return View();
        }

        public async Task<IActionResult> CreateUser(UserModel model)
        {
            var calculationService = new TrafficCalculationService();
            var createModel = model.CreateModel;

            if (createModel.Username == null || createModel.Username.Length < 4) 
            {
                return View("Admin", new UserModel() { CreateMessage = "Username needs to be 4 characters." }); 
            }

            if (createModel.Password == null || createModel.Password.Length < 6) 
            {
                return View("Admin", new UserModel() { CreateMessage = "Password needs to be 6 characters." });
            }

            var adminString = createModel.Admin ? "true" : "false";
            var apiKey = GetApiKey();
            var result = await calculationService.CreateUser(createModel.Username, createModel.Password, createModel.Name, adminString, apiKey);

            return View("Admin", new UserModel() { CreateMessage = result });
        }

        public async Task<IActionResult> DeleteUser(UserModel model)
        {
            var calculationService = new TrafficCalculationService();
            var deleteModel = model.DeleteModel;

            if (deleteModel.Username.Equals("admin")) 
            {
                return View("Admin", new UserModel() { DeleteMessage = "The admin user can't be deleted." });
            }

            var apiKey = GetApiKey();
            var result = await calculationService.DeleteUser(deleteModel.Username, apiKey);

            return View("Admin", new UserModel() { DeleteMessage = result });
        }

        public async Task<IActionResult> LoginAction(LoginModel model)
        {
            if (model.Username == null || model.Password == null || model.Username.Length < 4 || model.Password.Length < 6)
            {
                return View("Login", new LoginModel() { Message = "Invalid username or password." });
            }

            var calculationService = new TrafficCalculationService();
            var user = await calculationService.Login(model.Username, model.Password);

            if (user != null)
            {
                HttpContext.Session.Set("admin", Encoding.ASCII.GetBytes(user.admin.ToString()));
                HttpContext.Session.Set("apiKey", Encoding.ASCII.GetBytes(user.apiKey));
                return RedirectToAction("Home", "Traffic");
            }
                
            return View("Login", new LoginModel() { Message = "Login failed." } );
        }

        string GetApiKey()
        {
            byte[] apiKeyArray;
            HttpContext.Session.TryGetValue("apiKey", out apiKeyArray);
            return Encoding.ASCII.GetString(apiKeyArray);
        }

        bool IsAdmin()
        {
            byte[] byteArray;
            HttpContext.Session.TryGetValue("admin", out byteArray);
            return byteArray != null && Encoding.ASCII.GetString(byteArray).Equals(true.ToString());
        }
    }
}
