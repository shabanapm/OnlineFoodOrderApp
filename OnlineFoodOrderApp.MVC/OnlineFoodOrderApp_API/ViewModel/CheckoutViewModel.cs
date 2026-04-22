namespace OnlineFoodOrderApp.ViewModel
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal? CartTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal GrandTotal { get; set; }
        public string? RestaurantName { get; set; }
        public int RestaurantId { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerAddress { get; set; }

        public string? CustomerPhoneNo { get; set; }


    }
}
