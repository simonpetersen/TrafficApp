using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrafficApp.Models
{
    public class CreateUserModel
    {
        [DisplayName("Username")]
        [StringLength(50, MinimumLength = 4)]
        [Required]
        public string Username { get; set; }

        [DisplayName("Name")]
        [StringLength(50, MinimumLength = 4)]
        [Required]
        public string Name { get; set; }

        [DisplayName("Password")]
        [StringLength(100, MinimumLength = 8)]
        [Required]
        public string Password { get; set; }

        [DisplayName("Admin")]
        [Required]
        public bool Admin { get; set; }
    }
}
