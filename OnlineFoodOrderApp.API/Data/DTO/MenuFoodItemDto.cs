namespace FoodOrderingAPI.Data.DTO
{
    public class MenuFoodItemDto
    {
        public int FoodItemId { get; set; }
        public string? FoodItemName { get; set; }
        public string? FoodImage { get; set; }
        public decimal Unitprice { get; set; }
    }
}
