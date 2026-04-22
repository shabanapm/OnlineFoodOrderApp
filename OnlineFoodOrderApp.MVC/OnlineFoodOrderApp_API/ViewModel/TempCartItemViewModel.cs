namespace OnlineFoodOrderApp.ViewModel
{
    public class TempCartItemViewModel
    {
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
