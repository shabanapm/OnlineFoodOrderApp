namespace FoodOrderingAPI.Data.DTO
{
    public class MenuViewDto
    {
        public int RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public List<MenuFoodItemDto>? MenuItems { get; set; }
    }
}
