using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace web2020apr_p01_assignment_group5.Models
{
    public class PersonnelViewModel
    {
        public int StaffId { get; set; }

        [StringLength(50)]
        public string StaffName { get; set; }

        [StringLength(50)]
        public string Vocation { get; set; }

        [StringLength(255)]
        public string Status { get; set; }

        public List<FlightSchedule> flightScheduleList;
    }
}
