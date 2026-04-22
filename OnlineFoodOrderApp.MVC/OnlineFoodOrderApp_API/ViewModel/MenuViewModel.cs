using OnlineFoodOrderApp.Models;

namespace OnlineFoodOrderApp.ViewModel
{
    public class MenuViewModel
    {
        public int RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public List<MenuFoodItemViewModel>? MenuItems { get; set; }
    }
}
