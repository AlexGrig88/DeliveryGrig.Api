using DeliveryGrig.Api.Data;
using System.Text.RegularExpressions;

namespace DeliveryGrig.Api.Utils
{
    public class OrderValidator
    {
        private readonly DataContext _dataContext;

        public OrderValidator(DataContext context) => _dataContext = context;

        public bool ValidateWeight(string weight, out string message)
        {
            message = "Success";
            if (!double.TryParse(weight, out double result)) {
                message = "Некорректный формат для веса.";
                return false;
            }
            return true;
        }

        public bool ValidateDistrict(string district, out string message)
        {
            message = "Success";
            if (string.IsNullOrEmpty(district)) {
                message = "Поле должно иметь значение.";
                return false;
            }
            if (!_dataContext.Districts.Any(d => d == district)) {
                message = "Район доставки не найден.";
                return false;
            }
            return true;
        }

        public bool ValidateFirstDeliveryTime(string firstDeliveryDateTime, out string message)
        {
            message = "Success";
            if (string.IsNullOrEmpty(firstDeliveryDateTime)) {
                message = "Поле должно иметь значение.";
                return false;
            }
           
            if (!DateTime.TryParse(firstDeliveryDateTime, out var resultTime)) {
                message = "Невозможно распарсить время доставки, проверьте формат вводимых данных и корректность значений времени." +
                    "Требуется формат вида: yyyy-MM-dd HH:mm:ss";
                return false;
            }

            var yyyy = @"(\d{4})";
            var dd = @"(\d{2})";
            var deliveryTimeRegex = new Regex($"^{yyyy}-{dd}-{dd} {dd}:{dd}:{dd}$");   
            if (!deliveryTimeRegex.IsMatch(firstDeliveryDateTime)) {
                message = $"Проверьте формат и разделители между числами. Требуется формат вида: yyyy-MM-dd HH:mm:ss";
                return false;
            }

            var recievedYear = int.Parse(firstDeliveryDateTime.Split('-')[0]);
            if (recievedYear < DateTime.Now.Year || recievedYear > (DateTime.Now.Year + 1)) {   // более узкая специализации фильтра, которая зависит от бизнес-требований
                message = $"Измените год доставки. Доставка осуществляется в пределах текущего {DateTime.Now.Year} года и на следующий.";
                return false;
            }
            return true;
        }
    }
}
