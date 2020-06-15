using System;
using System.ComponentModel.DataAnnotations;

namespace web2020apr_p01_assignment_group5.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        
        [StringLength(50)]
        public string StaffName { get; set; }

        public char Gender { get; set; }

        public DateTime DateEmployed { get; set; }

        [StringLength(50)]
        public string Vocation { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public string Nationality { get; set; }

        [StringLength(255)]
        private string Password { get; set; }

        [StringLength(255)]
        private string Status { get; set; }
        
    }
}
