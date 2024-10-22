using DeliveryGrig.Api.Data;
using DeliveryGrig.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryGrig.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly ApplicationContext _dbContext;

        public ValuesController(ApplicationContext context)
        {
            _dbContext = context;
        }

        [HttpGet("test")]
        public List<Order> Test()
        {
            return _dbContext.Orders.ToList();
        }

    }
}
