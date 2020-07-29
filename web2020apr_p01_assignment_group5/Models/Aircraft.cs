using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web2020apr_p01_assignment_group5.Models
{
    public class Aircraft
    {
        public int AircraftId { get; set; }
        public string MakeModel { get; set; }
        public int NumEconomySeat { get; set; }
        public int NumBusinessSeat { get; set; }
        public DateTime DateLastMaintenance { get; set; }
        public string Status { get; set; }
    }
}
