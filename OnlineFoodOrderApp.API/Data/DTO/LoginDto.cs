namespace FoodOrderingAPI.Data.DTO
{
    public class LoginDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public List<TempCartItemDto> tempCartItemViewModels { get; set; }
    }
}
