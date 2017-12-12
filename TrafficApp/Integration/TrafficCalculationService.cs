using System;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using TrafficApp.Models;
using System.Net.Http.Headers;
using System.Xml;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace TrafficApp.Integration
{
    public class TrafficCalculationService
    {
        public static IConfigurationRoot Configuration { get; set; }

        public async Task<User> Login(string Username, string Password)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string baseUrl = Configuration["BaseUrl"] + "user/login/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                var hashPassword = ComputeHash(Password);

                client.DefaultRequestHeaders.Add("password", hashPassword);

                HttpResponseMessage Response = await client.PostAsync(Username, new StringContent(Password));
                if (Response.IsSuccessStatusCode)
                {
                    var UserResponse = Response.Content.ReadAsStringAsync().Result;

                    var StringReader = new StringReader(UserResponse);
                    var xRoot = new XmlRootAttribute();
                    xRoot.ElementName = "user";
                    xRoot.IsNullable = true;

                    var Serializer = new XmlSerializer(typeof(User),xRoot);
                    var user = (User)Serializer.Deserialize(StringReader);

                    return user;
                }
            }

            return null;
        }

        public async Task<string> CreateUser(string Username, string Password, string Name, string Admin, string apiKey) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string baseUrl = Configuration["BaseUrl"] + "user/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("name", Name);
                client.DefaultRequestHeaders.Add("admin", Admin);

                var hashPassword = ComputeHash(Password);
                client.DefaultRequestHeaders.Add("password", hashPassword);

                HttpResponseMessage Response = await client.PostAsync(Username + "?apiKey=" + apiKey, new StringContent(Password));
                if (Response.IsSuccessStatusCode)
                {
                    return "User successfully created.";
                }
            }

            return "Creation failed.";
        }

        public async Task<string> DeleteUser(string Username, string ApiKey)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            string baseUrl = Configuration["BaseUrl"] + "user/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();

                HttpResponseMessage Response = await client.DeleteAsync(Username + "?apiKey=" + ApiKey);
                if (Response.IsSuccessStatusCode)
                {
                    return "User successfully deleted.";
                }
            }

            return "Deletion failed.";
        }

        string ComputeHash(string value) {
            var hashBytes = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(value));
            var hashString = string.Empty;
            foreach (byte x in hashBytes)
            {
                hashString += String.Format("{0:x2}", x);
            }

            return hashString;
        }

    }
}
