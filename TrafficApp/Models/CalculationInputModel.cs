using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace TrafficApp.Models
{
    public class CalculationInputModel
    {
        [DisplayName("Start")]
        [StringLength(100, MinimumLength = 10)]
        [Required]
        public string StartAddress { get; set; }

        [DisplayName("Destination")]
        [StringLength(100, MinimumLength = 10)]
        [Required]
        public string DestinationAddress { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime JourneyDateTime { get; set; }
    }
}
