using DeliveryGrig.Api.Entities;
using Microsoft.AspNetCore.Builder;

namespace DeliveryGrig.Api.Data
{
    public class DataContext
    {
        private readonly IWebHostEnvironment _env;

        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<string> Districts { 
            get => GenerateValidDistrict().ToList();  // имитация получения списка существующих районов из бд для правила валидации
        }

        public DataContext(IWebHostEnvironment environment)
        {
            _env = environment;
            Orders = ReadOrdersFromFile("init_data.csv");
        }

        public IEnumerable<Order> ReadOrdersFromFile(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, "data_files", fileName);
            var orderList = new List<Order>();

            using (var reader = new StreamReader(filePath)) {
                var line = "";
                if (!reader.EndOfStream) reader.ReadLine(); // отбрасываем первую заголовочную строку
                while (!reader.EndOfStream) {
                    var values = reader.ReadLine()?.Split(';') ?? new string[] { };
                    orderList.Add(new Order(values));
                }
            }
            return orderList;
        }

        private IEnumerable<string> GenerateValidDistrict()
        {
            int postfix = 100;
            string[] bases = { "Nord", "West", "East", "South" };
            return Enumerable
                .Range(0, bases.Length * 4)
                .Select(i => $"{bases[i % 4]}{postfix * (i % 4 + 1)}");
        }
    }
}
