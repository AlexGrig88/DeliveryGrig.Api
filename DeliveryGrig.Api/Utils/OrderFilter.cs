using DeliveryGrig.Api.Controllers;
using DeliveryGrig.Api.Data;
using DeliveryGrig.Api.Dto;
using DeliveryGrig.Api.Entities;
using Microsoft.Extensions.Logging;

namespace DeliveryGrig.Api.Utils
{
    public class OrderFilter
    {
        public string CityDistrict { get; set; }
        public DateTime FirstDeliveryDateTime { get; set; }
        private readonly ILogger<OrderFilter> _logger;

        public OrderFilter(ILogger<OrderFilter> logger) { _logger = logger; }

        public OrderFilter SetupFromDto(OrderFilterDto filterDto)
        {
            CityDistrict = filterDto._cityDistrict;
            FirstDeliveryDateTime = DateTime.Parse(filterDto._firstDeliveryDateTime);
            return this;
        }

        public IEnumerable<Order> Apply(IEnumerable<Order> inputOrders)
        {
            var endDeliveryTime = FirstDeliveryDateTime.AddMinutes(30);
            _logger.LogInformation($"Фильтрация осуществлена. Временной интервал составляет: [с {FirstDeliveryDateTime} по {endDeliveryTime}]");
            return inputOrders
                .Where(ord => (ord.DeliveryTime >= FirstDeliveryDateTime && ord.DeliveryTime <= endDeliveryTime) && ord.District == CityDistrict);
        }
        
    }
}
