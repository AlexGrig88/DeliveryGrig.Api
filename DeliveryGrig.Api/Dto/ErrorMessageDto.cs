namespace DeliveryGrig.Api.Dto
{
    public class ErrorMessageDto
    {
        public string Message { get; set; } = string.Empty;
        public ErrorMessageDto(string msg)
        {
            Message = msg;
        }
    }
}
