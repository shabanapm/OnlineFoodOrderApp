namespace FoodOrderingAPI.Data.DTO
{
    public class OrderItemDto
    {
        public string FoodItemName { get; set; }
        public int? Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
