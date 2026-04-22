using System.ComponentModel.DataAnnotations;

namespace FoodOrderingAPI.Data.DTO
{
    public class ProfileDto
    {
        public string? CustomerName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
