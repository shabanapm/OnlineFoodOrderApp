using System.ComponentModel.DataAnnotations;

namespace OnlineFoodOrderApp_API.ViewModel
{
    public class RegisterViewModel
    {
        
        [StringLength(50)]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Password Required")]
        public string Password { get; set; }
        
        [StringLength(200)]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        
        [StringLength(20)]
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter valid 10-digit phone")]
        public string Phone { get; set; }
    }
}
