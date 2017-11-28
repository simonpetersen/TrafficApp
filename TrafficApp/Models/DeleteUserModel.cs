using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrafficApp.Models
{
    public class DeleteUserModel
    {
        [DisplayName("Username")]
        [StringLength(50, MinimumLength = 4)]
        [Required]
        public string Username { get; set; }
    }
}
