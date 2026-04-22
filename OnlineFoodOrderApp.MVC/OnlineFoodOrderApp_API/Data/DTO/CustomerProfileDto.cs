using OnlineFoodOrderApp.Models;

namespace OnlineFoodOrderApp.Data.DTO
{
    public class CustomerProfileDto
    {
        public bool success { get; set; }
        public string message { get; set; }
        public TblCstmr cstmr { get; set; }
    }
}
