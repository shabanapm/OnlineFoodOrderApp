using FoodOrderingAPI.Data;
using FoodOrderingAPI.Data.DTO;
using FoodOrderingAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly FoodOrderingDbContext _context;
        public OrderController(FoodOrderingDbContext dbContext)
        {
            _context = dbContext;
        }
        [HttpPost("createorder/{id}")]
        public IActionResult CreateOder(int id)
        {
            var cartMaster = _context.TblCartMaster.SingleOrDefault(c => c.CustomerId == id);

            if (cartMaster == null)
            {
                return Ok(0);
            }
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                var cartdetail = _context.TblCartDetails.Where(c => c.CartMasterId == cartMaster.CartMasterId).ToList();
                if (cartdetail.Count == 0)
                {
                    return Ok(0);
                }

                var orderMaster = new TblOrder
                {
                    //CartMasterId = cartMaster.CartMasterId,
                    OrderAmt = cartMaster.TotalAmt,
                    RestaurantId = cartMaster.RestaurantId,
                    CustomerId = cartMaster.CustomerId,
                    OrderStatus = "Placed",
                    OrderDate = DateTime.Now
                };
                _context.TblOrders.Add(orderMaster);
                _context.SaveChanges();
                var orderDetailList = new List<TblOrderDetail>();
                foreach (var item in cartdetail)
                {
                    var orderDetail = new TblOrderDetail
                    {
                        OrderId = orderMaster.OrderId,
                        FoodItemId = item.FoodItemId,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,
                        Quantity = item.Quantity
                    };
                    orderDetailList.Add(orderDetail);
                }

                _context.TblOrderDetails.AddRange(orderDetailList);
                _context.TblCartDetails.RemoveRange(cartdetail);
                _context.TblCartMaster.Remove(cartMaster);
                _context.SaveChanges();
                transaction.Commit();
                return Ok( orderMaster.OrderId );
            }
            catch
            {
                transaction.Rollback();
                throw;
            }


           
        }

        [HttpGet("getordersummary")]
        public IActionResult OrderSummary([FromQuery]int userId,[FromQuery]int orderId)
        {
            if(userId==0 || orderId==0)
                return BadRequest();
            var OrderSummaryVM = _context.TblOrders
                            .Where(o => o.OrderId == orderId && o.CustomerId == userId)
                            .Select(c => new OrderSummaryDto
                            {
                                OrderId = orderId,
                                OrderDate = c.OrderDate,
                                OrderAmount = c.OrderAmt,
                                OrderStatus = c.OrderStatus,
                                RestaurantName = c.Restaurant.RestaurantName,
                                DeliveryAddress = c.Customer.Address,
                                DeliveryFee = 5m,

                                Items = c.TblOrderDetails
                                              .Select(k => new CartItemDto
                                              {
                                                  FoodItemName = k.FoodItem.Name,
                                                  Quantity = k.Quantity??0,
                                                  UnitPrice = k.UnitPrice,
                                                  TotalPrice = k.TotalPrice,
                                              })
                                              .ToList()
                            }).SingleOrDefault();
            if (OrderSummaryVM == null)
            {
                return Ok(new OrderSummaryDto { Items = null });
            }
            return Ok(OrderSummaryVM);

        }

        [HttpGet("orderdisplay")]
        public IActionResult Orderdisplay([FromQuery] int userId )
        {
            if (userId == 0)
                return BadRequest();
            var orders = _context.TblOrders
                      .Where(c => c.CustomerId == userId)
                      .Include(c => c.Restaurant)
                      .Include(c => c.Customer)
                     .Include(c => c.TblOrderDetails)
                      .ThenInclude(d => d.FoodItem)
                      .ToList();

            if (!orders.Any())
                return NotFound("No orders found");
            try
            {

                foreach (var o in orders)
            {
                var orderDate = o.OrderDate;
                if (o.OrderStatus == "Delivered")
                    continue;
                var diff = DateTime.Now - orderDate;
                if (diff.TotalMinutes < 2)
                    o.OrderStatus = "Placed";
                else if (diff.TotalMinutes < 4)
                    o.OrderStatus = "Preparing";
                else if (diff.TotalMinutes < 6)
                    o.OrderStatus = "Out for Delivery";
                else
                    o.OrderStatus = "Delivered";
            }
            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error updating order status: " + ex.Message);
            }
           
            var viewmodelList = orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderStatus = o.OrderStatus,
                OrderAmount = o.OrderAmt,
                OrderDate = o.OrderDate,
                RestaurantName = o.Restaurant.RestaurantName,
                DeliveryAddress = o.Customer.Address,
                orderItemdto = o.TblOrderDetails.Select(c => new OrderItemDto
                {
                    FoodItemName = c.FoodItem.Name,
                    UnitPrice = c.UnitPrice,
                    Quantity = c.Quantity,
                    TotalPrice = c.TotalPrice,
                }).ToList()
            }).ToList();

            return Ok(viewmodelList);
        }

    }
}
