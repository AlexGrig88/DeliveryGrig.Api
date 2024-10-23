using DeliveryGrig.Api.Entities;

namespace DeliveryGrig.Api.Dto
{
    public record class OrderDto
    {
 
        public double Weight { get; init; }
        public string District { get; init; }
        public string DeliveryTime { get; init; }

        public OrderDto() { }
        public OrderDto(double weight, string district, string deliveryTime)
        {
            Weight = weight;
            District = district;
            DeliveryTime = deliveryTime;
        }

        public OrderDto(Order order)
        {
            Weight = order.Weight;
            District = order.District;
            DeliveryTime = order.DeliveryTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;
        }
    }
}
