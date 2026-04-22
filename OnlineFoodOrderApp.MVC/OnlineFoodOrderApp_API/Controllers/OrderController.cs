using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrderApp.Data;
using OnlineFoodOrderApp.Models;
using OnlineFoodOrderApp.Services;
using OnlineFoodOrderApp.ViewModel;
using System.Threading.Tasks;

namespace OnlineFoodOrderApp.Controllers
{
    public class OrderController : Controller
    {
     
        private readonly CartService _service;
        public OrderController(CartService service)
        { _service = service; }
           
       
        public async Task<IActionResult> OrderPlaced()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if(!userId.HasValue)
            {
                return RedirectToAction("LoginPageLoad", "Account");
            }

           var orderId=await _service.CreateOrderAsync(userId.Value);
            if (orderId == 0)
                return RedirectToAction("ViewCart", "Cart");
            return RedirectToAction("OrderSummary", new { orderId = orderId });
         

        }
        public async Task<IActionResult> OrderSummary(int orderId)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return Unauthorized();
            }

            var OrderSummaryVM =await _service.OrderSummaryAsync(userId.Value,orderId);
            if(OrderSummaryVM==null)
            {
                return RedirectToAction("ViewCart", "Cart");
            }
        
            return View(OrderSummaryVM);
        }

        public async Task<IActionResult> OrderDisplay()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue)
            {
                return RedirectToAction("LoginPageLoad", "Account");
            }
var viewmodelList=await _service.OrderDisplayAsync(userId.Value);

                return View(viewmodelList);
        }
    }
}
