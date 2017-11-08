using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using TrafficApp.Models;

namespace TrafficApp.Controllers
{
    public class NominatimGeocodingService
    {

        public async Task<Tuple<double, double>> GetCoordinates(string address) {
            string baseUrl = "http://nominatim.openstreetmap.org/search/";
            string urlExtension = "?format=xml&limit=1";

            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage Response = await client.GetAsync(address + urlExtension);
                if (Response.IsSuccessStatusCode) {
                    var GeoResponse = Response.Content.ReadAsStringAsync().Result;

                    XmlReader Reader = XmlReader.Create(new StringReader(GeoResponse));
                    while (Reader.Read()) {
                        var NodeType = Reader.NodeType;
                        if (NodeType.Equals(XmlNodeType.Element) && Reader.Name.Equals("place")) {
                            double? Latitude = GetValue(Reader, "lat");
                            double? Longitude = GetValue(Reader, "lon");

                            if (Latitude.HasValue && Longitude.HasValue)
                            {
                                return new Tuple<double, double>(Latitude.Value, Longitude.Value);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private double? GetValue(XmlReader Reader, string AttributeName)
        {
            while (Reader.MoveToNextAttribute())
            {
                if (Reader.Name.Equals(AttributeName))
                {
                    return Convert.ToDouble(Reader.Value);
                }
            }

            return null;
        }
    }
}
