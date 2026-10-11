using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommproject_utility127
{
    public class SD
    {
        //roles
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee user";
        public const string Role_company = "Company user";
        public const string Role_Individual = "Individual user";

        //Session
        public const string Ss_cartSessioncount = "cart count Session";

        public static double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
                return price;
            else if (quantity < 100)
                return price50;
            else return price100;
        }

        public static double GetPriceBasedOnQuantity(int count, string price, string price50)
        {
            throw new NotImplementedException();
        }

        //order status
        public const string orderstatusPending = "Pending";
        public const string orderstatusApproved = "Approved";
        public const string orderstatusInProgress = "Processing";
        public const string orderstatusShipped = "shipped";
        public const string orderstatusCancelled = "Cancelled";
        public const string orderstatusRefunded = "Refunded";

        // payment Status
        public const string PaymentstatusPending = "Pending";
        public const string PaymentstatusApproved = "Approved";
        public const string PaymentstatusDelayPayment = "PaymentstatusDelay";
        public const string PaymentstatusRejected = "Rejected";
    }
}
