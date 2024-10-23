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
                message = "Такого района не существует.";
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
            var yearReg = $"({DateTime.Now.Year}|{DateTime.Now.Year + 1})";            
            var monthReg = "(0[1-9]|1[0-2])";               
            var dayReg = "(0[1-9]|1[0-9]|2[0-9]|3[0-1])";    //  день парсится не совсем корректно (для всех мес. 31 день), но данную погрешность не пропустит первая проверка в условии if 
            var hourReg = "([0-1][0-9]|2[0-3])";           
            var minSecReg = "([0-5][0-9])";                                      
            var deliveryTimeRegex = new Regex($"^{yearReg}-{monthReg}-{dayReg} {hourReg}:{minSecReg}:{minSecReg}$");  // для более узкой специализации, которая зависит от бизнес-требований 
            if (!DateTime.TryParse(firstDeliveryDateTime, out var resultTime) || !deliveryTimeRegex.IsMatch(firstDeliveryDateTime)) {
                message = $"Некорректный формат для поля {nameof(firstDeliveryDateTime)}. Требуется формат вида: yyyy-MM-dd HH:mm:ss";
                return false;
            }
            return true;
        }
    }
}
