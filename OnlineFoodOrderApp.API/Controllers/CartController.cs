using Azure.Core;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.Data.DTO;
using FoodOrderingAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FoodOrderingAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly FoodOrderingDbContext _context;
        public CartController(FoodOrderingDbContext context)
        {
            _context = context;
        }
        [HttpPost("addtocart")]
        public IActionResult AddToCart([FromBody] AddToCartRequestDto request)
        {
            if (request == null || request.FoodItemId == 0 || request.RestId == 0)
            {
                return BadRequest("Invalid request");
            }
            if (request.UserId.HasValue)
            {

                var cartmaster = _context.TblCartMaster.FirstOrDefault(c => c.CustomerId == request.UserId);
                if (cartmaster != null)
                {
                    if (cartmaster.RestaurantId != request.RestId)
                    {
                        var result = new { success = false, mismatch = true, message = "Your cart contains items from another restaurant. Clear it and continue?" };
                        return BadRequest(result);

                    }
                    var existingCartDetail = _context.TblCartDetails.FirstOrDefault(c => c.CartMasterId == cartmaster.CartMasterId && c.FoodItemId == request.FoodItemId);

                    if (existingCartDetail == null)
                    {
                        var newcartDetail = new TblCartDetail
                        {
                            CartMasterId = cartmaster.CartMasterId,
                            FoodItemId = request.FoodItemId,
                            UnitPrice = request.UnitPrice,
                            TotalPrice = request.UnitPrice,
                            Quantity = 1,
                            DateAdded = DateTime.Now
                        };
                        _context.TblCartDetails.Add(newcartDetail);

                    }
                    else
                    {
                        existingCartDetail.Quantity += 1;
                        existingCartDetail.TotalPrice += request.UnitPrice;
                    }
                    cartmaster.TotalAmt += request.UnitPrice;
                }
                else
                {
                    cartmaster = new TblCartMaster
                    {

                        CartStatus = "Active",
                        CustomerId = request.UserId.Value,
                        RestaurantId = request.RestId,
                        TotalAmt = request.UnitPrice,
                        DateAdded = DateTime.Now,
                        TblCartDetails = new List<TblCartDetail>()

                    };

                    var cartDetail = new TblCartDetail
                    {

                        FoodItemId = request.FoodItemId,
                        UnitPrice = request.UnitPrice,
                        Quantity = 1,
                        TotalPrice = request.UnitPrice,
                        DateAdded = DateTime.Now
                    };
                    cartmaster.TblCartDetails.Add(cartDetail);//Adding CartDetail via cartMaster.CartDetails.Add(cartDetail) automatically sets CartId when SaveChanges() is called.
                    _context.TblCartMaster.Add(cartmaster); //No need for two _context.SaveChanges() calls.

                }
                _context.SaveChanges();
            }

            return Ok(new { success = true });
        }
        [HttpPost("gettocart")]
        public IActionResult GetToCart([FromBody] ViewCartRequest request)
        {

            if (request.UserId != 0)
            {

                var cartMaster = _context.TblCartMaster.FirstOrDefault(c => c.CustomerId == request.UserId);

                if (cartMaster != null)
                {
                    var restId = cartMaster.RestaurantId;
                    var cartDetail = _context.TblCartDetails.Include(c => c.FoodItem).Where(c => c.CartMasterId == cartMaster.CartMasterId).ToList();
                    var cartitemList = cartDetail.Select(item => new CartItemDto
                    {
                        ItemId = item.FoodItemId,
                        FoodItemName = item.FoodItem?.Name ?? "unknown item",
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice
                    }).ToList();

                    var cartresponse = new CartResponseDto
                    {
                        CartItems = cartitemList,
                        RestaurantId = restId,
                        RestaurantName = _context.TblRestaurants.FirstOrDefault(c => c.RestaurantId == restId)?.RestaurantName
                    };
                    return Ok(cartresponse);
                }
                else
                {
                    return Ok(new CartResponseDto() { CartItems = new List<CartItemDto>() });
                }
            }
            return BadRequest("Invalid request");

        }

        [HttpPost("getfoodnames")]
        public IActionResult GetFoodNames([FromBody] List<int> foodIds)
        {
            if (foodIds == null || !foodIds.Any())
                return Ok(new Dictionary<int, string>());
            try
            {
                var foodList = _context.TblFoodItems.Where(f => foodIds.Contains(f.FoodItemId))
                .ToDictionary(f => f.FoodItemId, f => f.Name);


                return Ok(foodList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }


        }
        [HttpPut("updatecart")]
        public IActionResult UpdateCart([FromBody] UpdateCartDto request)
        {
            if (request == null || request.FoodItemId == 0 || request.UserId == 0 || string.IsNullOrEmpty(request.Action))
            {
                return BadRequest("Invalid request");
            }
            var cartMaster = _context.TblCartMaster.SingleOrDefault(c => c.CustomerId == request.UserId);
            if (cartMaster != null)
            {
                var cartDetail = _context.TblCartDetails.SingleOrDefault(c => c.CartMasterId == cartMaster.CartMasterId && c.FoodItemId == request.FoodItemId);
                if (cartDetail != null)
                {
                    if (request.Action == "Add")
                    {
                        cartDetail.Quantity += 1;
                        cartDetail.TotalPrice += cartDetail.UnitPrice;
                        cartMaster.TotalAmt += cartDetail.UnitPrice;
                    }
                    else if (request.Action == "Remove" && cartDetail.Quantity > 0)
                    {

                        cartDetail.Quantity -= 1;
                        cartDetail.TotalPrice -= cartDetail.UnitPrice;
                        cartMaster.TotalAmt -= cartDetail.UnitPrice;

                        if (cartDetail.Quantity == 0)
                        {
                            _context.TblCartDetails.Remove(cartDetail);
                            _context.SaveChanges();
                            return Ok(new { success = true, qty = 0, total = 0 });

                        }
                    }
                    _context.SaveChanges();
                    var cartResponse = new UpdateCartResponseDto
                    {
                        Quantity = cartDetail.Quantity,
                        Price = cartDetail.TotalPrice
                    };
                    return Ok(cartResponse);
                }
            }
            return Ok(new UpdateCartResponseDto());
        }
        [HttpDelete("clearcart/{id}")]
        public IActionResult ClearCart(int id)
        {

            var cartmaster = _context.TblCartMaster
                            .Include(c => c.TblCartDetails)
                            .FirstOrDefault(c => c.CustomerId == id);
            if (cartmaster == null)
                return NotFound("Cart not found");
            else
            {
                if (cartmaster.TblCartDetails.Any())
                {
                    _context.TblCartDetails.RemoveRange(cartmaster.TblCartDetails);
                }
                _context.TblCartMaster.Remove(cartmaster);
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpPut("resolvecart")]
        public IActionResult Resolvecart([FromBody] ResolveCartConflictDto resolveCart)
        {
            if (resolveCart == null)
                return BadRequest();


            var cartMaster = _context.TblCartMaster
               .Include(c => c.TblCartDetails)
               .FirstOrDefault(c => c.CustomerId == resolveCart.UserId);
            _context.TblCartDetails.RemoveRange(cartMaster.TblCartDetails);
            if (cartMaster == null)
                return NotFound("Cart not found");
            cartMaster.RestaurantId = resolveCart.CartItems.First().RestaurantId;
            cartMaster.TotalAmt = resolveCart.CartItems.Sum(x => x.TotalPrice);

            var newDetails = resolveCart.CartItems.Select(x => new TblCartDetail
            {
                CartMasterId = cartMaster.CartMasterId,
                FoodItemId = x.ItemId,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                TotalPrice = x.Quantity * x.UnitPrice,
                DateAdded = DateTime.Now
            }).ToList();

            foreach (var item in newDetails)
                _context.TblCartDetails.Add(item);

            _context.SaveChanges();
            return Ok();
        }
        [HttpGet("checkoutcart/{id}")]
        public IActionResult CheckOutCartAsync(int id)
        {
            if (id == 0)
                return BadRequest();

            var checkout = _context.TblCartMaster
                     .Where(c => c.CustomerId == id)
                     .Select(c => new
                     {
                         cart = c,
                         customer = c.Customer
                     })
                     .Select(x => new CheckoutDto
                     {
                         CartItems = x.cart.TblCartDetails
                                   .Select(d => new CartItemDto
                                   {
                                       ItemId = d.FoodItemId,
                                       FoodItemName = d.FoodItem.Name,
                                       UnitPrice = d.UnitPrice,
                                       Quantity = d.Quantity,
                                       TotalPrice = d.TotalPrice
                                   }).ToList(),
                         CartTotal = x.cart.TotalAmt,
                         RestaurantName = x.cart.Restaurant.RestaurantName,
                         RestaurantId = x.cart.RestaurantId,
                         DeliveryFee = 5,
                         GrandTotal = x.cart.TotalAmt + 5,
                         CustomerName = x.customer.CustomerName,
                         CustomerAddress = x.customer.Address,
                         CustomerPhoneNo = x.customer.Phone
                     })

                     .SingleOrDefault();
            if (checkout == null)
                return Ok(new CheckoutDto { CartItems = new List<CartItemDto>() });

            return Ok(checkout);

        }
    }
}
