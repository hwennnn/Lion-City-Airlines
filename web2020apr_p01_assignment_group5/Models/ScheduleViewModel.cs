using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web2020apr_p01_assignment_group5.Models
{
    public class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public string FlightNumber { get; set; }
        public int RouteId { get; set; }
        public string AircraftId { get; set; }
        public DateTime? DepartureDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public double EconomyClassPrice { get; set; }
        public double BusinessClassPrice { get; set; }
        public string Status { get; set; }
        public FlightRoute Route { get; set; }
    }
}
