namespace DeliveryGrig.Api.Dto
{
    public record class OrderDto(
        int Id,
        double Weight,
        string District,
        string DeliveryTime
        );
}
