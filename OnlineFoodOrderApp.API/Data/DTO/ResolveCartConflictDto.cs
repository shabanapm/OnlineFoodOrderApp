namespace FoodOrderingAPI.Data.DTO
{
    public class ResolveCartConflictDto
    {
        public int UserId { get; set; }
        public List<TempCartItemDto> CartItems { get; set; }
    }
}
