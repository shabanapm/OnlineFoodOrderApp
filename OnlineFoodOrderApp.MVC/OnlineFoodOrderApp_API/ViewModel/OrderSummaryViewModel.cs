namespace OnlineFoodOrderApp.ViewModel
{
    public class OrderSummaryViewModel
    {
        public int OrderId {  get; set; }
        public DateTime OrderDate {  get; set; }
        public string OrderStatus {  get; set; }
        public decimal OrderAmount {  get; set; }
        public string RestaurantName {  get; set; }
        public string DeliveryAddress {  get; set; }
        public decimal DeliveryFee { get; set; }
       
        public List<OrderItemViewModel> Items { get; set; }
        public decimal grandTotal=>OrderAmount+DeliveryFee;
    }
}
