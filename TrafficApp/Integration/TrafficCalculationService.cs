using System;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using TrafficApp.Models;
using System.Net.Http.Headers;
using System.Xml;
using System.IO;

namespace TrafficApp.Integration
{
    public class TrafficCalculationService
    {
        public async Task<User> Login(string Username, string Password)
        {
            string baseUrl = "http://localhost:8080/rs/user/login/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                client.DefaultRequestHeaders.Add("password", Password);

                HttpResponseMessage Response = await client.PostAsync(Username, new StringContent(Password));
                if (Response.IsSuccessStatusCode)
                {
                    var UserResponse = Response.Content.ReadAsStringAsync().Result;

                    XmlReader Reader = XmlReader.Create(new StringReader(UserResponse));
                    Reader.MoveToContent();
                    var user = new User();
                    while (Reader.Read())
                    {
                        if (Reader.Name.Equals("admin")) {
                            user.IsAdmin = Reader.ReadElementContentAsBoolean();
                        } else if (Reader.Name.Equals("name")) {
                            user.Name = Reader.ReadElementContentAsString();
                        }
                    }

                    return user;
                }
            }

            return null;
        }

    }
}
