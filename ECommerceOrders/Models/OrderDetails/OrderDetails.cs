using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceOrders.Models.OrderDetails
{
    public class OrderDetails
    {
        public CustomerDetails Customer { get; set; }
        public CustomerOrder Order { get; set; }
    }
}
