using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommproject_Model127
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            count = 1;
        }
        public int Id { get; set; }
        public string Applicationuserid { get; set; }
        [ForeignKey("Applicationuserid")]
        public ApplicationUser Applicationuser { get; set; }
        public int Productid { get; set; }
        [ForeignKey("Productid")]
        public Product Product { get; set; }
        public int count { get; set; }
        [NotMapped] 
        public double Price { get; set; }
    }
}
