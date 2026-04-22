using System.Text.Json.Serialization;

namespace OnlineFoodOrderApp.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId {  get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public  string? RestaurantName { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal OrderAmount { get; set; }
        [JsonPropertyName("orderItemdto")]  // maps JSON "orderItemdto" → this property
        public List<OrderItemViewModel> orderItemViewModels { get; set; }
    }
}
