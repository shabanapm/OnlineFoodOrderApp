using FoodOrderingAPI.Data;
using FoodOrderingAPI.Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.Model;

namespace FoodOrderingAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly FoodOrderingDbContext _context;
        public RestaurantController(FoodOrderingDbContext dbContext)
        {
            _context = dbContext;

        }
        [HttpGet("getmenu")]
        public IActionResult GetMenu([FromQuery] int restId)
        {
            if(restId==0)
            {
                return BadRequest();
            }
            try
            {
                var restaurantName = _context.TblRestaurants.Where(r => r.RestaurantId == restId).Select(r => r.RestaurantName).FirstOrDefault();
                if(restaurantName==null)
                { return NotFound("Restaurant not found"); }
                var menu = _context.TblRestaurantFudItems
                      .Where(rf => rf.RestaurantId == restId)
                       .Select(rf => new MenuFoodItemDto
                       {
                           FoodItemId = rf.FoodItemId,
                           FoodItemName = rf.FoodItem.Name,
                           FoodImage = rf.FoodItem.ImagePath,
                           Unitprice = rf.FoodPrice
                       }).ToList();

                var FoodMenu = new MenuViewDto
                {
                    RestaurantId = restId,
                    RestaurantName = restaurantName,
                    MenuItems = menu
                };


                return Ok(FoodMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("loadrestaurant")]
        public IActionResult LoadRestaurant()
        {
            var restList = _context.TblRestaurants.ToList();
            if(restList==null || !restList.Any())
                return Ok(new List<object>());
            var dtoList = restList.Select(r => new
            {
                restaurantId = r.RestaurantId,
                restaurantName = r.RestaurantName,
                email = r.Email,
                phone = r.Phone,
                address = r.Address
            }).ToList();
            return Ok(dtoList);
        }

    }
}
