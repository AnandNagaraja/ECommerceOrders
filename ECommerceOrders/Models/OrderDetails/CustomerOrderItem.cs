namespace ECommerceOrders.Models.OrderDetails
{
    public class CustomerOrderItem
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal? PriceEach { get; set; }
    }
}