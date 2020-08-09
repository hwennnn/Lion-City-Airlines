using System;
using System.ComponentModel.DataAnnotations;

namespace web2020apr_p01_assignment_group5.Models
{
    public class FlightSchedule
    {
        public int ScheduleId { get; set; }

        [StringLength(20)]
        public string FlightNumber { get; set; }

        public int RouteId { get; set; }
        public string AircraftId { get; set; }
        public DateTime? DepartureDateTime { get; set; }

        public DateTime ArrivalDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double EconomyClassPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double BusinessClassPrice { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public string Role { get; set; }
    }
}
