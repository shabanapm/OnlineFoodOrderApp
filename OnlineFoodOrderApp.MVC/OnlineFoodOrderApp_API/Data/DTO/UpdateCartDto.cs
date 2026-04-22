namespace OnlineFoodOrderApp.Data.DTO
{
    public class UpdateCartDto
    {
        public int UserId { get; set; }
        public int FoodItemId { get; set; }
        public string Action { get; set; }
    }
}
