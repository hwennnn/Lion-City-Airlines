using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web2020apr_p01_assignment_group5.Models
{
    public class BookingModel
    {
        public List<SelectListItem> departureCountryList { get; set; }

        public List<SelectListItem> arrivalCountryList { get; set; }
    }
}
