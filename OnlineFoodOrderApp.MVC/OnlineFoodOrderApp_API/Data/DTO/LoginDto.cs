using OnlineFoodOrderApp.ViewModel;

namespace OnlineFoodOrderApp.Data.DTO
{
    public class LoginDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public List<TempCartItemViewModel> tempCartItemViewModels { get; set; } = new List<TempCartItemViewModel>();
    }
}
