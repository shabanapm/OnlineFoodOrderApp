namespace FoodOrderingAPI.Data.DTO
{
   
        public class TempCartItemDto
        {
            public int ItemId { get; set; }
            public int RestaurantId { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
        }
    
}
