using Microsoft.AspNetCore.Mvc;
using SalesAPI.Data;
using SalesAPI.DTO;
using SalesAPI.Models;
using SalesAPI.Repository;

namespace SalesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly SalesDbContext _context;

        public OrdersController(OrderService orderService,SalesDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult CreateOrderOrAddProduct([FromBody] OrderCreationDTO orderCreationDTO)
        {
            try
            {
                if(_context.Accounts.FirstOrDefault(a=>a.AccountNumber==orderCreationDTO.AccountID)==null)
                {
                    throw new Exception("Account not found");
                }
                if(orderCreationDTO.OrderID!=0)
                {
                    try
                    {
                        _orderService.AddProductToOrder((int)orderCreationDTO.OrderID, orderCreationDTO.ProductId, orderCreationDTO.Quantity);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                
                var order = new Order
                {
                    AccountID = orderCreationDTO.AccountID,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    CarrierTrackingNumber = string.Empty,
                    SalesOrderDetails = new List<SalesOrderDetailEntity>()
                };

                var orderId = _orderService.CreateOrder(orderCreationDTO);
                return Ok($"Order created successfully with ID: {orderId}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("total/{orderId}")]
        public IActionResult GetOrderTotal(int orderId)
        {
            try
            {
                var total = _orderService.GetOrderTotal(orderId);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get all orders/{accountId}")]
        public IActionResult GetOrdersByAccountId(int accountId)
        {
            try
            {
                var orders = _orderService.GetAllOrdersByAccountId(accountId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
