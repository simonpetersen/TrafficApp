using System;
namespace TrafficApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public bool IsAdmin { get; set; }
    }
}
