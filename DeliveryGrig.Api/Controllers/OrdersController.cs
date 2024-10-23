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

        private readonly DataContext _dataContext;

        public OrdersController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet("test")]
        public List<Order> Test()
        {
            return _dataContext.Orders.ToList();
        }

    }
}
