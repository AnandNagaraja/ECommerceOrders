using System.Collections.Generic;

namespace ECommerceOrders.Models.OrderDetails
{
    public class CustomerOrder
    {
        public int OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<CustomerOrderItem> OrderItems { get; set; }
        public string DeliveryExpected { get; set; }
    }
}