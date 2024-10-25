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

            var distConfig = _configuration["FilteringParams:_cityDistrict"];
            var firstDeliveryConfig = _configuration["FilteringParams:_firstDeliveryDateTime"];

            // в случае получения параметров запроса из файла конфигурации, первой запрос выполняем с конфиг параметрами, игнорируя полученные от клиента
            if (!string.IsNullOrEmpty(distConfig) && !string.IsNullOrEmpty(firstDeliveryConfig)) {
                filterDto = new OrderFilterDto() 
                { _cityDistrict = distConfig,
                  _firstDeliveryDateTime = firstDeliveryConfig,
                  _recordsQuantity = 20
                };
            }

            if (filterDto._recordsQuantity <= 0 || filterDto._recordsQuantity > 20) {
                var msg = $"Запрос не прошёл валидацию: Колличество записей должно быть в диапазоне [0:20]";
                _logger.LogError(msg);
                return BadRequest(new ErrorMessageDto(msg));
            }

            _logger.LogInformation($"Вызова метода POST по пути ресурса api/Orders/filter. " +
                $"Данные для фильтрации: [_cityDistrict = \"{filterDto._cityDistrict}\", " +
                $"_firstDeliveryDateTime = \"{filterDto._firstDeliveryDateTime}\"]");

            try {
                _orderFilterValidator.Validate(filterDto);
            }
            catch (OrderFilterException ex) {
                _logger.LogError($"Запрос не прошёл валидацию: [{ex.Message}]");
                return BadRequest(new ErrorMessageDto(ex.Message));
            }

            var orders = _filter.SetupFromDto(filterDto)
                .Apply(_dataContext.Orders)
                .Take(filterDto._recordsQuantity);

            if (orders == null || orders.Count() == 0) {
                var errorMsg = "Записей с данными параметрами фильтрации не найдено.";
                _logger.LogError(errorMsg);
                return NotFound(new ErrorMessageDto(errorMsg));
            }


            try {
                await _dataContext.SaveResultsAsync(orders.ToList());   // сохранение результа в файл
            }
            catch (IOException ex) { 
                _logger.LogError($"{ex.Message}");
            }

            return Ok(orders.Select(ord => new OrderDto(ord)).ToList());
        }

    }
}
