using System.ComponentModel.DataAnnotations;

namespace DeliveryGrig.Api.Dto
{
    public class OrderFilterDto
    {
        [StringLength(40, ErrorMessage = "Длина строки не может быть длинней 40 символов")]
        public string _cityDistrict { get; init; }
        [StringLength(40, ErrorMessage = "Длина строки не может быть длинней 40 символов")]
        public string _firstDeliveryDateTime { get; init; }
        public int _recordsQuantity { get; init; }
    }
}
