using DeliveryGrig.Api.Entities;
using Microsoft.AspNetCore.Builder;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace DeliveryGrig.Api.Data
{
    public class DataContext
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _RootPath;
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<string> Districts { 
            get => GenerateValidDistricts().ToList();  // имитация получения списка существующих районов из бд для правила валидации
        }
        

        public DataContext(IWebHostEnvironment environment)
        {
            _env = environment;
            _RootPath = Path.Combine(_env.WebRootPath, "data_files");
            Orders = ReadOrdersFromFile(Path.Combine(_RootPath, "init_data.csv"));
        }

        public IEnumerable<Order> ReadOrdersFromFile(string filePath)
        {
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

        private IEnumerable<string> GenerateValidDistricts()
        {
            int postfix = 100;
            string[] bases = { "Nord", "West", "East", "South" };
            return Enumerable
                .Range(0, bases.Length * 4)
                .Select(i => $"{bases[i % 4]}{postfix * (i / 4 + 1)}");
        }

        public async Task SaveResultsAsync(List<Order> orders)
        {
            FileInfo fileInfo = new FileInfo("result_data.txt");
            bool isAppended = true;
            if (!fileInfo.Exists) {
                isAppended = false;
            }
            string header = "";
            if (!isAppended) header = "Id;Weight;District;DeliveryTime\n\n";
            string timeStamp = $"======Time stamp of record: {DateTime.Now}======\n";
            string resultData = header + timeStamp + string.Join("\n", orders.Select(ord => GetCsvLine(ord)).ToArray());
            string dataSeparator = new string('=', 50);
            using (StreamWriter writer = new StreamWriter(fileInfo.FullName, isAppended)) {
                await writer.WriteLineAsync(resultData);
            }
        }

        private string GetCsvLine(Order order) => $"{order.Id};{order.Weight};{order.District}:{order.DeliveryTime}";
    }
}
