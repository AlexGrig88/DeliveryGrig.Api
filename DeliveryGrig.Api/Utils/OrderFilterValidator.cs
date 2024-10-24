using DeliveryGrig.Api.Data;
using DeliveryGrig.Api.Dto;
using DeliveryGrig.Api.Exeptions;
using System.Text.RegularExpressions;

namespace DeliveryGrig.Api.Utils
{
    public class OrderFilterValidator
    {
        private readonly DataContext _dataContext;
        /*private string _exceptionMessage;*/

        public OrderFilterValidator(DataContext context) => _dataContext = context;


        public void Validate(OrderFilterDto filterDto)
        {
            string exceptionMsg = string.Empty;
            ValidateDistrict(filterDto._cityDistrict, ref exceptionMsg);
            ValidateFirstDeliveryTime(filterDto._firstDeliveryDateTime, ref exceptionMsg);
            if (exceptionMsg != string.Empty) {
                throw new OrderFilterException(exceptionMsg);
            }
        }


        private void ValidateDistrict(string district, ref string exceptionMessage)
        {
            if (string.IsNullOrEmpty(district)) {
                exceptionMessage += "Поле \"Район\" должно иметь значение.";
            }
            else if (!_dataContext.Districts.Any(d => d == district)) {
                exceptionMessage += "Район доставки не найден.";
            }
        }

        private void ValidateFirstDeliveryTime(string firstDeliveryDateTime, ref string exceptionMessage)
        {
            if (string.IsNullOrEmpty(firstDeliveryDateTime)) {
                exceptionMessage += " Поле \"Время первой доставки\" должно иметь значение.";
                return;
            }
            if (!DateTime.TryParse(firstDeliveryDateTime, out var resultTime)) {
                exceptionMessage += " Невозможно распарсить Время первой доставки, проверьте формат вводимых данных и корректность значений времени." +
                    "Требуется формат вида: yyyy-MM-dd HH:mm:ss";
                return;
            }
            var yyyy = @"(\d{4})";
            var dd = @"(\d{2})";
            var deliveryTimeRegex = new Regex($"^{yyyy}-{dd}-{dd} {dd}:{dd}:{dd}$");   
            if (!deliveryTimeRegex.IsMatch(firstDeliveryDateTime)) {
                exceptionMessage += " Проверьте формат и разделители между числами. Требуется формат вида: yyyy-MM-dd HH:mm:ss";
                return;
            }

            var recievedYear = int.Parse(firstDeliveryDateTime.Split('-')[0]);
            if (recievedYear < DateTime.Now.Year || recievedYear > (DateTime.Now.Year + 1)) {   // более узкая специализации фильтра, которая зависит от бизнес-требований
                exceptionMessage += $" Измените год доставки. Доставка осуществляется в пределах текущего {DateTime.Now.Year} года и на следующий.";
            }
        }
    }
}
