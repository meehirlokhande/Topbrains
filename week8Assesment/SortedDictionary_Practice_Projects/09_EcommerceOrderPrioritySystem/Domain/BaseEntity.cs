namespace Domain
{
    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public int OrderAmount { get; set; }

        public Order(string orderId, string customerName, int orderAmount)
        {
            OrderId = orderId;
            CustomerName = customerName;
            OrderAmount = orderAmount;
        }
    }
}
