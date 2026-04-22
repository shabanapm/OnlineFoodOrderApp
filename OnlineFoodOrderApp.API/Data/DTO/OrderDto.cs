namespace FoodOrderingAPI.Data.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? RestaurantName { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal OrderAmount { get; set; }
        public List<OrderItemDto> orderItemdto { get; set; }
    }
}
