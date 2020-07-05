using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web2020apr_p01_assignment_group5.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public int ScheduleId { get; set; }

        [StringLength(50)]
        public string PassengerName { get; set; }

        [StringLength(20)]
        public string PassportNumber { get; set; }

        [StringLength(50)]
        public string Nationality { get; set; }

        [StringLength(20)]
        public string SeatClass { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double AmtPayable { get; set; }

        [StringLength(3000)]
        public string Remarks { get; set; }

        public DateTime DateTimeCreated { get; set; }
    }
}