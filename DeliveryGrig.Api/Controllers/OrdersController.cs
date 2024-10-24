using DeliveryGrig.Api.Data;
using DeliveryGrig.Api.Dto;
using DeliveryGrig.Api.Entities;
using DeliveryGrig.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryGrig.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly DataContext _dbContext;
        private readonly OrderFilter _filter;
        private readonly OrderValidator _orderValidator;

        public OrdersController(DataContext context, OrderFilter orderFilter, OrderValidator orderValidator)
        {
            _dbContext = context;
            _orderValidator = orderValidator;
            _filter = orderFilter;
        }

        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            return Ok(_dbContext.Orders.Select(ord => new OrderDto(ord)).ToList());
        }

        [HttpPost("filter")]
        public IActionResult GetFilteredOrders([FromBody] OrderFilterDto filterDto)
        {
            var errorMsg = string.Empty;
            if (!_orderValidator.ValidateDistrict(filterDto._cityDistrict, out string errorDistr)) {
                errorMsg += errorDistr;
            }
            if (!_orderValidator.ValidateFirstDeliveryTime(filterDto._firstDeliveryDateTime, out string errorDelivery)) {
                errorMsg += $"\n{errorDelivery}";
            }

            if (!string.IsNullOrEmpty(errorMsg)) {
                return BadRequest(new ErrorMessageDto(errorMsg));
            }

            var ordersDto = _filter
                .SetupFromDto(filterDto)
                .Apply(_dbContext.Orders)
                .Select(ord => new OrderDto(ord))
                .Take(filterDto._recordsQuantity)
                .ToList();

            if (ordersDto == null || ordersDto.Count == 0) {
                return NotFound(new ErrorMessageDto("Записей с данными параметрами фильтрации не найдено."));
            }
            return Ok(ordersDto);
        }

    }
}
