
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommproject_Model127
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public String Name { get; set; }
        [Display(Name = "street Address")]
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        [Display(Name = "Postal code")]
        public string Postalcode { get; set; }
        [Display(Name = "company")]
        public int? companyid { get; set; }
        [ForeignKey("companyid")]
        public Company company { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
