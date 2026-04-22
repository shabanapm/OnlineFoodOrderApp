namespace OnlineFoodOrderApp.ViewModel
{
    public class ViewCartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal? CartTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal GrandTotal { get; set; }
        public string? RestaurantName { get; set; }
        public int RestaurantId { get; set; }
    }
}
