using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommproject_Model127.ViewModels
{
    public class ShoppingcartVM
    {
        public IEnumerable<ShoppingCart> Listcart { get; set; }
        public OrderHeader orderHeader { get; set; }
    }
}
