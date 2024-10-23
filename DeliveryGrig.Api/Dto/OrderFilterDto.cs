namespace DeliveryGrig.Api.Dto
{
    public record class OrderFilterDto
    {
        public string _cityDistrict { get; init; }
        public string _firstDeliveryDateTime { get; init; }
    }
}
