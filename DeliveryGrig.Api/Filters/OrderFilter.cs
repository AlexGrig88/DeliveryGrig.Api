using DeliveryGrig.Api.Data;

namespace DeliveryGrig.Api.Filters
{
    public class OrderFilter
    {
        private readonly DataContext _dataContext;
        
        public string CityDistrict { get; set; }
        public string FirstDeliveryDateTime { get; set; }

        public OrderFilter(DataContext context)
        {
            _dataContext = context;
        }

        public void ValidateDistrict(string district)
        {

        }

        public void ValidateFirstDelivery(string firstDelivery)
        {

        }
    }
}
