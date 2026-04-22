using FoodOrderingAPI.Data;
namespace FoodOrderingAPI.Data.DTO
{
    public class CartResponseDto
    {
        public List<CartItemDto> CartItems { get; set; }
        public string RestaurantName { get; set; }
        public int RestaurantId { get; set; }
    }
}
