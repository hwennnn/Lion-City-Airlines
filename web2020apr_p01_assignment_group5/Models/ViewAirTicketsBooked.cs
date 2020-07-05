using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic.CompilerServices;
using web2020apr_p01_assignment_group5.DAL;

namespace web2020apr_p01_assignment_group5.Models
{
    public class ViewAirTicketsBooked
    {
        public int BookingId { get; set; }

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
        public string? Remarks { get; set; }

        public DateTime DateTimeCreated { get; set; }

        [StringLength(20)]
        public string FlightNumber { get; set; }

        [StringLength(50)]
        public string DepartureCity { get; set; }

        [StringLength(50)]
        public string DepartureCountry { get; set; }

        public DateTime DepartureDateTime { get; set; }

        [StringLength(50)]
        public string ArrivalCity { get; set; }

        [StringLength(50)]
        public string ArrivalCountry { get; set; }

        public DateTime ArrivalDateTime { get; set; }
    }
}
