namespace Page.API.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
