using System;
namespace TrafficApp.Models
{
    public class UserModel
    {
        public CreateUserModel CreateModel { get; set; }
        public DeleteUserModel DeleteModel { get; set; }
        public string CreateMessage { get; set; }
        public string DeleteMessage { get; set; }
    }
}
