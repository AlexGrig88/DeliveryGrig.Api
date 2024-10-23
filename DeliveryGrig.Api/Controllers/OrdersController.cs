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
        public List<Order> GetAllOrders()
        {
            return _dbContext.Orders.ToList();
        }

        [HttpPost("filter")]
        public IActionResult GetFilteredOrders([FromBody] OrderFilterDto filterDto)
        {
            var errorMessage = "";
            if (!_orderValidator.ValidateDistrict(filterDto.CityDistrict, out errorMessage)) {
                return BadRequest(errorMessage);
            }
            if (!_orderValidator.ValidateFirstDeliveryTime(filterDto.FirstDeliveryDateTime, out errorMessage)) {
                return BadRequest(errorMessage);
            }
            var orders = _filter.SetupFromDto(filterDto)
                .Apply(_dbContext.Orders)
                .ToList();
  
            return Ok(orders);
        }

    }
}
