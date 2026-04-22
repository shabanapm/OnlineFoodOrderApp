namespace OnlineFoodOrderApp.ViewModel
{
    public class OrderItemViewModel
    {
        public string FoodItemName { get; set; }
        public int? Quantity {  get; set; }
        public decimal UnitPrice {  get; set; }
        public decimal TotalPrice { get; set; }
    }

}
