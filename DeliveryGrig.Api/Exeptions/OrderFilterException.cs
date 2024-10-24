namespace DeliveryGrig.Api.Exeptions
{
    public class OrderFilterException : Exception
    {
        public OrderFilterException(string message)
        : base(message) { }
    }
}
