using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineFoodOrderApp.Data;
using OnlineFoodOrderApp.Models;
using OnlineFoodOrderApp.Services;
using OnlineFoodOrderApp.ViewModel;
using OnlineFoodOrderApp_API.ViewModel;
using System.Threading.Tasks;

namespace OnlineFoodOrderApp.Controllers
{
    public class AccountController : Controller
    {
      
        private readonly CartService _service;
        public AccountController(CartService service)
        {
          
            _service = service;
        }
        public IActionResult LoginPageLoad(string returnurl)
        {

            ViewBag.ReturnUrl = returnurl;
            return View();
        }
        public async Task<IActionResult> LoginClick(LoginViewModel model,string returnUrl)
        {
            var tempCartJson = HttpContext.Session.GetString("TempCartSession");
            var CartList = !string.IsNullOrEmpty(tempCartJson)
               ? JsonConvert.DeserializeObject<List<TempCartItemViewModel>>(tempCartJson)
               : new List<TempCartItemViewModel>();
            var loginDto = new Data.DTO.LoginDto
            {
                UserName = model.UserName,
                Password = model.Password,
                tempCartItemViewModels = CartList
            };
            var apiResult = await _service.LoginClickAsync(loginDto);
            if (apiResult.userId != 0)
            {
                HttpContext.Session.SetInt32("userId", apiResult.userId);
                    if (apiResult.conflict==true)
                    {
                        return RedirectToAction("ViewCart", "Cart", new { conflict = true });

                    }
                        HttpContext.Session.Remove("TempCartSession");
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Load_Restaurants", "Restaurant");
            }
          else
                TempData["message"] = apiResult.message;
            return RedirectToAction("LoginPageLoad");
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Load_Restaurants", "Restaurant");
        }

        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            
            if (!userId.HasValue)
            {
                return RedirectToAction("LoginPageLoad", "Account");
            }
            try
            {
                var customer = await _service.ProfileLoadAsync(userId.Value);
                if (customer == null || string.IsNullOrEmpty(customer.CustomerName))
                {
                    TempData["error"] = "Failed to load profile.";
                    return RedirectToAction("Load_Restaurants", "Restaurant");
                }
              
                    var customerViewmodel = new CustomerViewModel
                    {
                        CustomerName = customer.CustomerName,
                        Address = customer.Address,
                        Email = customer.Email,
                        Phone = customer.Phone

                    };
                    return View(customerViewmodel);
               
            }
            catch (Exception ex) 
            {
                TempData["error"] = "Something went wrong. Please try again.";
                return RedirectToAction("Load_Restaurants", "Restaurant");

            }
        }
        public IActionResult UserRegistration()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserRegistration_Click(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
                var serviceCall = await _service.UserRegisterAsyn(registerViewModel);

                if (serviceCall.success == true)
                {
                    HttpContext.Session.SetInt32("userId", serviceCall.userId);
                    TempData["msg"] = serviceCall.message;
                    return RedirectToAction("Load_Restaurants", "Restaurant");

                }
                ModelState.AddModelError("", serviceCall.message);
                return View("UserRegistration", registerViewModel);

        }
    }
}
