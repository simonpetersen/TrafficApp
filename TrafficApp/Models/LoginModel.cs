using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrafficApp.Models
{
    public class LoginModel
    {
        [DisplayName("Username")]
        [StringLength(100, MinimumLength = 4)]
        [Required]
        public string Username { get; set; }

        [DisplayName("Password")]
        [StringLength(100, MinimumLength = 8)]
        [Required]
        public string Password { get; set; }

        public string Message { get; set; }
    }
}
