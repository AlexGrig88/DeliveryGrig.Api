using DeliveryGrig.Api.Entities;
using Microsoft.AspNetCore.Builder;

namespace DeliveryGrig.Api.Data
{
    public class DataContext
    {
        public IEnumerable<Order> Orders { get; set; }
        public DataContext()
        {
            Orders = new List<Order>()
            {
                new Order() { Id = 1, Weight = 1.5, District = "Nord100", DeliveryTime = DateTime.Now },
                new Order() { Id = 2, Weight = 3.75, District = "West123", DeliveryTime = new DateTime(2024, 9, 25, 22, 45, 0) }
            };
        }
    }
}
