namespace DeliveryGrig.Api.Dto
{
    public record class OrderDto(
        int Id,
        double Weigth,
        string District,
        DateTime DeliveryTime
        );

}
