using System;
using System.Collections.Generic;

namespace web2020apr_p01_assignment_group5.Models
{
    public class ViewPersonnelsModel
    {
        public List<FlightSchedule> flightScheduleList { get; set; }
        public List<Staff> staffList { get; set; }

        public ViewPersonnelsModel()
        {
            flightScheduleList = new List<FlightSchedule>();
            staffList = new List<Staff>();
        }
    }
    
}
