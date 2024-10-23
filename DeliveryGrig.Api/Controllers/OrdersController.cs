using DeliveryGrig.Api.Data;
using DeliveryGrig.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryGrig.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly DataContext _dbContext;

        public OrdersController(DataContext context)
        {
            _dbContext = context;
        }

        [HttpGet("all")]
        public List<Order> GetAllOrders()
        {
            return _dbContext.Orders.ToList();
        }

    }
}
