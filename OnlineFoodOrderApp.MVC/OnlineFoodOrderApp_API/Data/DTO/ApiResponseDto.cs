namespace OnlineFoodOrderApp.Data.DTO
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public bool Mismatch { get; set; } // optional, if your API sends it
        public string Message { get; set; }
    }
}
