namespace DeliveryGrig.Api.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public string District { get; set; }
        public DateTime? DeliveryTime { get; set; }

        public Order() { }
        
        public Order(int id, double weight, string district, DateTime deliveryTime)
        {
            Weight = weight;
            District = district;
            DeliveryTime = deliveryTime;
        }
    }
}
