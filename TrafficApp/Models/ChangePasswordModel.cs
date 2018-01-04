using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrafficApp.Models
{
    public class ChangePasswordModel
    {
        [DisplayName("Username")]
        [StringLength(50, MinimumLength = 4)]
        [Required]
        public string Username { get; set; }

        [DisplayName("Password")]
        [StringLength(100, MinimumLength = 8)]
        [Required]
        public string Password { get; set; }
    }
}
