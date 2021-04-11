using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using ECommerceOrders.Exception;
using ECommerceOrders.Models;
using ECommerceOrders.Services;


namespace ECommerceOrders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost("GetOderDetailsByCustomerInfo")]

        public async Task<IActionResult> GetOderDetailsByCustomerInfo([FromBody] CustomerInfo customerInfo)
        {
            if (string.IsNullOrWhiteSpace(customerInfo.CustomerId) || string.IsNullOrWhiteSpace(customerInfo.User))
            {
                return BadRequest("Invalid customer information, please correct and try again");
            }

            try
            {
                return new JsonResult(await _orderService.GetOrderDetailsByCustomerInfoAsync(customerInfo)
                    .ConfigureAwait(false));
            }
            catch (ECommerceValidationException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }



    }
}
