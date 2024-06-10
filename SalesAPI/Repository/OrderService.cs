using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.DTO;
using SalesAPI.Interfaces;
using SalesAPI.Models;

namespace SalesAPI.Repository
{
    public class OrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly SalesDbContext _dbContext;

        public OrderService( IProductRepository productRepository, SalesDbContext dbContext)
        {
            _productRepository = productRepository;
            _dbContext = dbContext;
        }

        public int CreateOrder(OrderCreationDTO orderCreationDTO)
        {
            if (orderCreationDTO == null)
            {
                throw new ArgumentNullException(nameof(orderCreationDTO), "Order cannot be null");
            }

            var product = _productRepository.Read(orderCreationDTO.ProductId);
            if (product == null || product.Qty < orderCreationDTO.Quantity)
            {
                throw new Exception("Product not available or insufficient stock");
            }

            var lineTotal = product.Price * orderCreationDTO.Quantity;
            var orderToCreate = new Order
            {
                AccountID = orderCreationDTO.AccountID,
                OrderDate = DateTime.Now,
                TotalAmount = lineTotal,
                CarrierTrackingNumber =GenerateTrackingNumber(),
                SalesOrderDetails = new List<SalesOrderDetailEntity>()
            };



            var orderDetail = new SalesOrderDetailEntity
            {
                OrderQty = orderCreationDTO.Quantity,
                LineTotal = lineTotal,
                Date = DateTime.Now,
                CarrierTrackingNumber=orderToCreate.CarrierTrackingNumber,
                Order = orderToCreate,
                OrderID =orderToCreate.OrderID,
                Product = product,
                ProductID=product.ProductID
    };
            
            orderToCreate.SalesOrderDetails = new List<SalesOrderDetailEntity> { orderDetail };
            _dbContext.Orders.Add(orderToCreate);
            _dbContext.SaveChanges();

            _productRepository.DecreaseQuantity(orderCreationDTO.ProductId, orderCreationDTO.Quantity);

            return orderToCreate.OrderID;
        }
        public string GenerateTrackingNumber()
        {
            Random rand = new Random();
            string number = "";
            for (int i = 0; i < 2; i++)
            {
                number += (char)rand.Next(97, 123);
            }
            number += "-";
            number += rand.Next(100, 1000).ToString();

            return number;
        }

        public void AddProductToOrder(int orderId, int productId, int quantity)
        {
            var order = _dbContext.Orders
                .Include(o => o.SalesOrderDetails)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            var product = _productRepository.Read(productId);
            if (product == null || product.Qty < quantity)
            {
                throw new Exception("Product not available or insufficient stock");
            }

            var lineTotal = product.Price * quantity;

            var orderDetailDTO = new SalesOrderDetailEntityDTO
            {
                OrderID = order.OrderID,
                OrderQty = quantity,
                LineTotal = lineTotal,
            };
            var orderDetail = new SalesOrderDetailEntity
            {
                OrderQty = quantity,
                LineTotal = lineTotal,
                Date = DateTime.Now,
                CarrierTrackingNumber = order.CarrierTrackingNumber,
                Order = order,
                OrderID = order.OrderID,
                Product = product,
                ProductID = product.ProductID
            };
            order.AccountID = order.AccountID;
            order.CarrierTrackingNumber = order.CarrierTrackingNumber;
            order.OrderDate = DateTime.Now;
            order.OrderID = order.OrderID;
            order.SalesOrderDetails.Add(orderDetail);
            _productRepository.DecreaseQuantity(productId, quantity);
            order.TotalAmount += lineTotal;
            _dbContext.SaveChanges();
        }


        public decimal GetOrderTotal(int orderId)
        {
            
                return _dbContext.SalesOrderDetailEntities
                               .Where(s => s.OrderID == orderId)
                               .Sum(s => s.LineTotal);
            
        }
        public IEnumerable<OrderDTO> GetAllOrdersByAccountId(int accountId)
        {
            var orders = _dbContext.Orders
                .Where(o => o.AccountID == accountId)
                .Select(o => new OrderDTO
                {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    AccountID = o.AccountID,
                    TotalAmount = o.TotalAmount,
                    CarrierTrackingNumber = o.CarrierTrackingNumber,
                    OrderDetails = o.SalesOrderDetails
                        .Select(d => new OrderDetailDTO
                        {
                            ProductID = d.ProductID.GetValueOrDefault(),
                            ProductName = d.Product != null ? d.Product.Name : string.Empty,
                            OrderQty = d.OrderQty,
                            LineTotal = d.LineTotal
                        }).ToList()
                }).ToList();

            return orders;
        }



    }

}

