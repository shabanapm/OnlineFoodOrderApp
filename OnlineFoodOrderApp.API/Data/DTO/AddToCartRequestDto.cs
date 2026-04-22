namespace FoodOrderingAPI.Data.DTO
{
    public class AddToCartRequestDto
    {
        public int? UserId { get; set; }
        public int FoodItemId {  get; set; }
        public int RestId { get; set; }
        public decimal UnitPrice {  get; set; }
    }
}
