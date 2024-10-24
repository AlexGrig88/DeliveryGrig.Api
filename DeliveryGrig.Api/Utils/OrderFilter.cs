using DeliveryGrig.Api.Data;
using DeliveryGrig.Api.Dto;
using DeliveryGrig.Api.Entities;

namespace DeliveryGrig.Api.Utils
{
    public class OrderFilter
    {
        public string CityDistrict { get; set; }
        public DateTime FirstDeliveryDateTime { get; set; }

        public OrderFilter() { }

        public OrderFilter SetupFromDto(OrderFilterDto filterDto)
        {
            CityDistrict = filterDto._cityDistrict;
            FirstDeliveryDateTime = DateTime.Parse(filterDto._firstDeliveryDateTime);
            return this;
        }

        public IEnumerable<Order> Apply(IEnumerable<Order> inputOrders)
        {
            var endDeliveryTime = FirstDeliveryDateTime.AddMinutes(30);
            return inputOrders
                .Where(ord => (ord.DeliveryTime >= FirstDeliveryDateTime && ord.DeliveryTime <= endDeliveryTime) && ord.District == CityDistrict);
        }
        
    }
}
