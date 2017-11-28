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

        /*
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
        */

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
                client.DefaultRequestHeaders.Add("password", Password);
                client.DefaultRequestHeaders.Add("name", Name);
                client.DefaultRequestHeaders.Add("admin", Admin);

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

    }
}
