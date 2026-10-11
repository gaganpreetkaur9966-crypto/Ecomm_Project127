using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommproject_Model127
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int orderHeaderId { get; set; }
        [ForeignKey("orderHeaderId")]
        public OrderHeader OrderHeader { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int count { get; set; }
        public double Price { get; set; }
    }
}
