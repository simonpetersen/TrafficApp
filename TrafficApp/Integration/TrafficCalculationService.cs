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

        public async Task<Route> CalculateRoute(double startLat, double startLon, double destLat, double destLon, DateTime date, string apiKey)
        {
            string baseUrl = "http://localhost:8080/rs/route/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                var url = startLat + "/" + startLon + "/" + destLat + "/" + destLon + "/" + date.ToString("yyyy-MM-dd HH:mm:ss") 
                                                                                                + "?apiKey=" + apiKey;
                
                HttpResponseMessage Response = await client.GetAsync(url);
                if (Response.IsSuccessStatusCode)
                {
                    var UserResponse = Response.Content.ReadAsStringAsync().Result;

                    XmlReader Reader = XmlReader.Create(new StringReader(UserResponse));
                    Reader.MoveToContent();
                    var nodeList = new ArrayList();
                    Node node = new Node();

                    while (Reader.Read())
                    {
                        if (Reader.NodeType == XmlNodeType.Element)
                        {
                            if (Reader.Name.Equals("id"))
                            {
                                node.NodeId = Reader.ReadElementContentAsInt();
                            }
                            else if (Reader.Name.Equals("longitude"))
                            {
                                node.Longitude = Reader.ReadElementContentAsDouble();
                            }
                        } else if (Reader.NodeType == XmlNodeType.Text) {
                            node.Latitude = Reader.ReadContentAsDouble();
                        }

                        if (node.NodeId != 0 && !node.Latitude.Equals(0) && !node.Longitude.Equals(0))
                        {
                            nodeList.Add(node);
                            node = new Node();
                        }
                    }

                    var route = new Route();
                    route.NodeList = nodeList;

                    return route;
                }
            }

            return null;
        }

    }
}
