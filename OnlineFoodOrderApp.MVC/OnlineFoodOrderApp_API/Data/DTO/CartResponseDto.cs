using OnlineFoodOrderApp.ViewModel;

namespace OnlineFoodOrderApp.Data.DTO
{
    public class CartResponseDto
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public string RestaurantName { get; set; }
        public int RestaurantId {  get; set; }

    }
}
