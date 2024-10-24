using DeliveryGrig.Api.Data;
using DeliveryGrig.Api.Dto;
using DeliveryGrig.Api.Entities;
using DeliveryGrig.Api.Exeptions;
using DeliveryGrig.Api.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryGrig.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private readonly OrderFilter _filter;
        private readonly OrderFilterValidator _orderFilterValidator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(DataContext context, OrderFilter orderFilter, OrderFilterValidator orderValidator,
            ILogger<OrdersController> logger, IConfiguration configuration)
        {
            _dataContext = context;
            _orderFilterValidator = orderValidator;
            _filter = orderFilter;
            _logger = logger;
            _configuration = configuration;

        }

        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            return Ok(_dataContext.Orders.Select(ord => new OrderDto(ord)).ToList());
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFilteredOrders([FromBody] OrderFilterDto filterDto)
        {
            _logger.LogInformation($"Вызова метода POST по пути ресурса api/Orders/filter. " +
                $"Данные для фильтрации: [_cityDistrict = \"{filterDto._cityDistrict}\", " +
                $"_firstDeliveryDateTime = \"{filterDto._firstDeliveryDateTime}\"]");

            _logger.LogDebug($"CommandLine Args: _cityDistrict={_configuration["_cityDistrict"]}; _firstDeliveryDateTime={_configuration["_firstDeliveryDateTime"]}");

            try {
                _orderFilterValidator.Validate(filterDto);
            }
            catch (OrderFilterException ex) {
                _logger.LogError($"Запрос не прошёл валидацию: [{ex.Message}]");
                return BadRequest(new ErrorMessageDto(ex.Message));
            }

            var orders = _filter
                .SetupFromDto(filterDto)
                .Apply(_dataContext.Orders)
                .Take(filterDto._recordsQuantity);

            if (orders == null || orders.Count() == 0) {
                var errorMsg = "Записей с данными параметрами фильтрации не найдено.";
                _logger.LogError(errorMsg);
                return NotFound(new ErrorMessageDto(errorMsg));
            }
            await _dataContext.SaveResultsAsync(orders.ToList());   // сохранение результа в файл

            return Ok(orders.Select(ord => new OrderDto(ord)).ToList());
        }

    }
}
