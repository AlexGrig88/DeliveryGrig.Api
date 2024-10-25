using DeliveryGrig.Api.Entities;
using Microsoft.AspNetCore.Builder;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace DeliveryGrig.Api.Data
{
    public class DataContext
    {
        private const string DefaultFilePath = "result_data.txt";
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public IEnumerable<Order> Orders { get; set; }

        public IEnumerable<string> Districts { 
            get => GenerateValidDistricts().ToList();  // имитация получения списка существующих районов из бд для правила валидации
        }
        

        public DataContext(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _env = environment;
            _config = configuration;
            Orders = ReadOrdersFromFile(Path.Combine(_env.WebRootPath, "data_files", "init_data.csv"));
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
            var filePath = _config["PathToResultRecords:_deliveryOrder"];
            if (string.IsNullOrEmpty(filePath)) {
                filePath = DefaultFilePath;
            }
            FileInfo fileInfo = new FileInfo(filePath);
            bool isAppended = true;
            if (!fileInfo.Exists) {
                isAppended = false;
            }
            string header = "";
            if (!isAppended) header = "Id\t\tWeight\t\tDistrict\t\tDeliveryTime\n";
            string timeStamp = $"\n=========Time stamp of record: {DateTime.Now}===========\n";
            string resultData = header + timeStamp + string.Join("\n", orders.Select(ord => GetFormatLine(ord)).ToArray());
            try {
                using (StreamWriter writer = new StreamWriter(fileInfo.FullName, isAppended)) {
                    await writer.WriteLineAsync(resultData);
                }
            }
            catch (IOException ex) {
                throw new IOException("Не удалось записать файл по указанному пути. Проверте корректность заданного пути.");
            }

        }

        private string GetFormatLine(Order order)
        {
            return $"{string.Format("{0,-8}", order.Id)}\t{string.Format("{0,-8}", order.Weight)}" +
                $"\t{string.Format("{0,-8}", order.District)}\t{string.Format("{0,-8}", order.DeliveryTime)}";
        } 
    }
}
