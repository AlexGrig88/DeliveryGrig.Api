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
            Id = id;
            Weight = weight;
            District = district;
            DeliveryTime = deliveryTime;
        }

        public Order(params string[] fieldsValues)
        {
            if (fieldsValues.Length != 0) {
                try {
                    Id = Convert.ToInt32(fieldsValues[0]);
                    Weight = Convert.ToDouble(fieldsValues[1]);
                    District = fieldsValues[2];
                    DeliveryTime = DateTime.Parse(fieldsValues[3]);
                }
                catch (FormatException ex) {
                    Console.WriteLine("Не удалось распарсить значение.\n" + ex.Message);
                    throw;
                }
            }
        }

        public override string ToString() => $"Order (Id = {Id}; Weight = {Weight}; District = {District}; DeliveryTime = {DeliveryTime})";
    }
}
