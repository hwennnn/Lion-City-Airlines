using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace web2020apr_p01_assignment_group5.Models
{
    public class FlightRoute
    {
        public int RouteId { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureCountry { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalCountry { get; set;}

        [Range(1, 24, ErrorMessage = "Flight Duration cannot exceed 24 hours!")]
        public int? FlightDuration { get; set; }
    }
}
