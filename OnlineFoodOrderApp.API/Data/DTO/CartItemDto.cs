namespace FoodOrderingAPI.Data.DTO
{
    public class CartItemDto
    {
        public int ItemId { get; set; }
        public string FoodItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
