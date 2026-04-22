namespace OnlineFoodOrderApp.Data.DTO
{
    public class ApiLoginResponseDto
    {
        public bool success { get; set; }
        public int userId { get; set; }
        public bool conflict {  get; set; }
        public string message { get; set; }

    }
}
