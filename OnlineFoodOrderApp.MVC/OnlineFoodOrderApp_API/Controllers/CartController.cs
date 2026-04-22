using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineFoodOrderApp.Data;
using OnlineFoodOrderApp.Data.DTO;
using OnlineFoodOrderApp.Models;
using OnlineFoodOrderApp.Services;
using OnlineFoodOrderApp.ViewModel;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;


namespace OnlineFoodOrderApp.Controllers
{
    public class CartController : Controller
    {
      
        private readonly CartService _cartService;
       
        public CartController(CartService cartService)
        {
            
            _cartService = cartService;
        }
    

        [HttpPost]
        public async Task<IActionResult> AddToCart(int foodItemId, int restaurantId, decimal unitPrice)
        {
            var userId = HttpContext.Session.GetInt32("userId");
           if(userId.HasValue)
            { 
            // Prepare the DTO (MVC version)
            var request = new AddToCartRequestDto
            {
                FoodItemId = foodItemId,
                RestId = restaurantId,
                UnitPrice = unitPrice,
                UserId = userId

            };

            // Call the API through CartService

            try
            {
                var apirespone = await _cartService.AddToCartAsync(request);
                return Json(apirespone);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
            else
            {
                List<TempCartItemViewModel> tempcart = new List<TempCartItemViewModel>();
                var tempcartJson = HttpContext.Session.GetString("TempCartSession");
                if (!string.IsNullOrEmpty(tempcartJson) && tempcartJson != "[]")
                {
                    tempcart = JsonConvert.DeserializeObject<List<TempCartItemViewModel>>(tempcartJson);
                    if (restaurantId != tempcart.First().RestaurantId)
                    {
                        return Json(new { success = false, mismatch = true, message = "Your cart contains items from another restaurant. Clear it and continue?" });
                    }

                    var existingCart = tempcart.FirstOrDefault(c => c.ItemId == foodItemId);
                    
                  
                    if (existingCart != null)
                    {
                        
                            existingCart.Quantity += 1;
                        existingCart.TotalPrice += unitPrice;
                    }
                    else
                    {
                        var temporaryItemVM = new TempCartItemViewModel
                        {
                            ItemId = foodItemId,
                            RestaurantId = restaurantId,
                            Quantity = 1,
                            UnitPrice = unitPrice,
                            TotalPrice = unitPrice
                        };
                        tempcart.Add(temporaryItemVM);
                    }
                }
                else
                {
                    var temporaryItemVM = new TempCartItemViewModel
                    {
                        ItemId = foodItemId,
                        RestaurantId = restaurantId,
                        Quantity = 1,
                        UnitPrice = unitPrice,
                        TotalPrice = unitPrice
                    };
                    tempcart.Add(temporaryItemVM);
                    
                }
                HttpContext.Session.SetString("TempCartSession", JsonConvert.SerializeObject(tempcart));
                return Json(new { success = true });
            }
        }
        public async Task<IActionResult> ViewCart(bool conflict = false)
        {
            ViewBag.Conflict = conflict;
            List<CartItemViewModel> cartitemList = new List<CartItemViewModel>();
           
            var userId = HttpContext.Session.GetInt32("userId");
            int restId = 0;
            var restname = "";
            if (userId.HasValue && userId.Value != 0)
            {
                var request = new ViewCartRequestDto
                {
                    UserId=userId.Value

                };

                try
                {
                    var apirespone = await _cartService.GetCartAsync(request);
                    cartitemList = apirespone.CartItems;
                    restId = apirespone.RestaurantId;
                    restname = apirespone.RestaurantName;
                  
                  
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                var tempCartJson = HttpContext.Session.GetString("TempCartSession");
                var CartList = !string.IsNullOrEmpty(tempCartJson)
                     ? JsonConvert.DeserializeObject<List<TempCartItemViewModel>>(tempCartJson)
                     : new List<TempCartItemViewModel>();
                restId = CartList.Any() ? CartList.First().RestaurantId : 0;
               
                var foodIdsList=CartList.Select(f=>f.UnitPrice).ToList();
                var foodIds = CartList.Select(f => f.ItemId).ToList();
                var foodNames = await _cartService.GetFoodNameAsync(foodIds);

                cartitemList = CartList.Select(item => new CartItemViewModel
                {
                    ItemId = item.ItemId,
                    FoodItemName = foodNames.ContainsKey(item.ItemId) ? foodNames[item.ItemId] : "Unknown Item",
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice
                }).ToList();

            }
            decimal cartTotal = cartitemList.Sum(x => x.TotalPrice);
            decimal deliveryFee = 5;  // or dynamic
            decimal grandTotal = cartTotal + deliveryFee;
     
            var viewcartList = new ViewCartViewModel
            {
                CartItems = cartitemList,
                CartTotal = cartTotal,
                DeliveryFee = deliveryFee,
                GrandTotal = grandTotal,
                RestaurantId = restId,
                RestaurantName = restname
            };

            return View("ViewCart", viewcartList);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart(int ItemId, string action)
        {
            int qty = 0;
            decimal total = 0;
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId != null)
            {
                try
                {
                    var cartDto = new UpdateCartDto
                    {
                        Action = action,
                        FoodItemId=ItemId,
                        UserId=userId.Value

                    };
                    var apiResponse =await _cartService.UpdateCartAsync(cartDto);
                    qty = apiResponse.Quantity;
                    total = apiResponse.Price;
                }
                catch(Exception ex ) 
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                var TempCart = HttpContext.Session.GetString("TempCartSession");
                if (TempCart != null)
                {
                    var tempcartJson = JsonConvert.DeserializeObject<List<TempCartItemViewModel>>(TempCart);
                    if (tempcartJson != null)
                    {
                        var Cart = tempcartJson.FirstOrDefault(c => c.ItemId == ItemId);
                        if (Cart != null)
                        {
                            if (action == "Add")
                            {
                                Cart.Quantity += 1;
                                Cart.TotalPrice += Cart.UnitPrice;
                            }
                            else if (action == "Remove" && Cart.Quantity > 0)
                            {

                                Cart.Quantity -= 1;
                                Cart.TotalPrice -= Cart.UnitPrice;

                                if (Cart.Quantity == 0)
                                {

                                    tempcartJson.Remove(Cart);
                                   
                                   
                                }
                            }
                            qty = Cart.Quantity;
                            total = Cart.TotalPrice;

                        }
                        if (!tempcartJson.Any())
                            HttpContext.Session.Remove("TempCartSession");
                        else
                            HttpContext.Session.SetString("TempCartSession", JsonConvert.SerializeObject(tempcartJson));

                    }
                }
            }

            return Json(new
            {
                success = true,
                qty = qty,
                total = total,
            });
        }

        public async Task<IActionResult> Checkout()
        {

            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {

                return RedirectToAction("LoginPageLoad", "Account", new { returnurl = Url.Action("Checkout", "Cart") });
            }
            else
            {

                var checkout = await _cartService.CheckOutAsync(userId.Value);
                return View(checkout);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = HttpContext.Session.GetInt32("userId");

            if (userId.HasValue && userId.Value != 0)
            {
                var apiResponse=await _cartService.ClearCartAsync(userId.Value);
                if (!apiResponse)
                    return Json(new { success = false, message = "Failed to clear cart" });

            }
            else
            {
                HttpContext.Session.Remove("TempCartSession");

            }

                return Json(new {success=true});
        }
        public async Task<IActionResult> ResolveCartConflict(string Choice)
        {
            if(Choice=="Keep")
            {
                HttpContext.Session.Remove("TempCartSession");
            }
            else if(Choice== "Replace")
            {
                var userId = HttpContext.Session.GetInt32("userId");

                var tempCartJson = HttpContext.Session.GetString("TempCartSession");

                var CartList = !string.IsNullOrEmpty(tempCartJson)
                    ? JsonConvert.DeserializeObject<List<TempCartItemViewModel>>(tempCartJson)
                    : new List<TempCartItemViewModel>();
                var resolve = new ResolveCartConflictDto
                {
                    UserId=userId.Value,
                    CartItems=CartList
                };
                var cartService=await _cartService.ResolvecartAsync(resolve);
            
              if(cartService)
                    HttpContext.Session.Remove("TempCartSession");
              else
                    TempData["Error"] = "Failed to resolve cart conflict";
            }
            return RedirectToAction("ViewCart");
        }
    }
}
