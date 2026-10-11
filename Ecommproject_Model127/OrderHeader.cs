using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommproject_Model127
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string ApplicationuserId { get; set; }
        [ForeignKey("ApplicationuserId")]
        public ApplicationUser Applicationuser { get; set; }
        [Required]
        public DateTime orderDate { get; set; }
        [Required]
        public DateTime shippingDate { get; set; }
        [Required]
        public double orderTotal { get; set; }
        public string TrackingNumber { get; set; }
        public string carrier { get; set; }
        public string orderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string TransactionId { get; set; }
        public String Name { get; set; }
        [Required]
        [Display(Name = "street Address")]
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        [Required]
        [Display(Name = "Postal code")]
        public string Postalcode { get; set; }
    }
}
