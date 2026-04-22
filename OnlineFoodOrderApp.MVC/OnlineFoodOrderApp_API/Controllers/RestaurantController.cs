using Microsoft.AspNetCore.Mvc;
using OnlineFoodOrderApp.Data;
using OnlineFoodOrderApp.Models;
using OnlineFoodOrderApp.Services;
using OnlineFoodOrderApp.ViewModel;
using System.Threading.Tasks;


namespace OnlineFoodOrderApp.Controllers
{
    public class RestaurantController : Controller
    {
        
        private readonly CartService _service;
        public RestaurantController(CartService service)
        {
           
            _service = service;
        }
        public async Task<IActionResult> Load_Restaurants()
        {
            var restList = await _service.LoadRestaurantAsync();
            return View(restList);

            //load all rests
                                  //apply search filter if only user enetered any text
                                  //if (!string.IsNullOrEmpty(SearchRestaurant))
                                  //{
                                  //    restList = _context.TblRestaurants.Where(r => r.RestaurantName.Contains(SearchRestaurant)).ToList();
                                  //}
                                  //ViewBag.SearchText = SearchRestaurant;

        }
        public async Task<IActionResult> GetMenu(int RestId)
        {
          var FoodMenu=await _service.GetMenuAsync(RestId);
            return View(FoodMenu);                                                   
        }
    }
}
