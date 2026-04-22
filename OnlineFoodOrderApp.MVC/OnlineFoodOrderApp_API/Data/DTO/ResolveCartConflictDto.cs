using OnlineFoodOrderApp.ViewModel;

namespace OnlineFoodOrderApp.Data.DTO
{
    public class ResolveCartConflictDto
    {
        public int UserId { get; set; }
        public List<TempCartItemViewModel> CartItems { get; set; }
    }
}
