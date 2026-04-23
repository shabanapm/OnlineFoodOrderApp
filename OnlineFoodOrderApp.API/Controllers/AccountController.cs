using BCrypt.Net;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.Data.DTO;
using FoodOrderingAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;

namespace FoodOrderingAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly FoodOrderingDbContext _context;
        public AccountController(FoodOrderingDbContext dbContext)
        {
            _context = dbContext;
        }
        [HttpPost("loginclick")]
        public IActionResult LoginClick([FromBody] LoginDto loginDto)
        {
            //if (loginDto == null)
            //{ return BadRequest(); }
            //// Step 1 — Find user by USERNAME only (not password!)
            //var customer = _context.TblCstmrs
            //                       .FirstOrDefault(c => c.UserName == loginDto.UserName);

            //// Step 2 — Verify password separately using BCrypt
            //if (customer == null)
            //    return Ok(new { success = false, message = "Invalid credentials." });

            //bool isValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.Password);

            //if (!isValid)
            //    return Ok(new { success = false, message = "Invalid credentials." });

            //// ✅ Login successful — continue with cart merge logic
            //var userId = customer.CustomerId;
            ////var userId = _context.TblCstmrs.Where(c => c.UserName == loginDto.UserName && c.Password == loginDto.Password).Select(c => c.CustomerId).FirstOrDefault();
            //if (userId != 0)
            //{
            //    var cartMaster = _context.TblCartMaster.SingleOrDefault(c => c.CustomerId == userId);
            //    var cartDetail = new List<TblCartDetail>();
            //    var CartList = loginDto.tempCartItemViewModels;
            //    if (cartMaster == null && CartList.Any())
            //    {
            //        var cartmaster = new TblCartMaster
            //        {

            //            CartStatus = "Active",
            //            CustomerId = userId,
            //            RestaurantId = CartList.First().RestaurantId,
            //            TotalAmt = CartList.Sum(x => x.TotalPrice),
            //            DateAdded = DateTime.Now,
            //            TblCartDetails = new List<TblCartDetail>()

            //        };

            //        cartDetail = CartList.Select(c => new TblCartDetail
            //        {

            //            FoodItemId = c.ItemId,
            //            UnitPrice = c.UnitPrice,
            //            Quantity = c.Quantity,
            //            TotalPrice = c.TotalPrice,
            //            DateAdded = DateTime.Now
            //        }).ToList();

            //        foreach (var detail in cartDetail)
            //        {
            //            cartmaster.TblCartDetails.Add(detail);
            //        }
            //        _context.TblCartMaster.Add(cartmaster); //No need for two _context.SaveChanges() calls.
            //        _context.SaveChanges();
            //    }
            //    else if (cartMaster != null && CartList.Any())
            //    {
            //        if (cartMaster.RestaurantId != CartList.First().RestaurantId)
            //        {
            //            return Ok(new { success = true, conflict = true, userId = userId });

            //        }
            //        else
            //        {
            //            foreach (var item in CartList)
            //            {
            //                var existingCart = cartMaster.TblCartDetails.FirstOrDefault(c => c.FoodItemId == item.ItemId);
            //                if (existingCart != null)
            //                {
            //                    existingCart.Quantity += item.Quantity;
            //                    existingCart.TotalPrice = existingCart.Quantity * existingCart.UnitPrice;
            //                }
            //                else
            //                {
            //                    var cart = new TblCartDetail
            //                    {
            //                        FoodItemId = item.ItemId,
            //                        UnitPrice = item.UnitPrice,
            //                        Quantity = item.Quantity,
            //                        TotalPrice = item.Quantity * item.UnitPrice,
            //                        DateAdded = DateTime.Now
            //                    };
            //                    cartMaster.TblCartDetails.Add(cart);
            //                }
            //            }
            //            cartMaster.TotalAmt = cartMaster.TblCartDetails.Sum(x => x.TotalPrice);
            //            _context.SaveChanges();
            //        }
            //    }
            //    return Ok(new { success = true, conflict = false, userId = userId });
            //}

            //else
            //{
            //    return Ok(new { success = false, conflict = false, userId = 0, message = "Login Failed" });
            //}
        
            if (loginDto == null)
                return BadRequest();

            // Step 1 — Find user by username only
            var customer = _context.TblCstmrs
                                   .FirstOrDefault(c => c.UserName == loginDto.UserName);

            if (customer == null)
                return Ok(new { success = false, message = "Invalid credentials." });

            // Step 2 — Verify password with BCrypt
            bool isValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.Password);

            if (!isValid)
                return Ok(new { success = false, message = "Invalid credentials." });

            // ✅ Login successful
            var userId = customer.CustomerId;

            // Step 3 — Cart merge logic
            var cartMaster = _context.TblCartMaster
                                     .Include(c => c.TblCartDetails)  // ✅ Include added
                                     .SingleOrDefault(c => c.CustomerId == userId);

            var CartList = loginDto.tempCartItemViewModels;

            if (cartMaster == null && CartList.Any())
            {
                var cartmaster = new TblCartMaster
                {
                    CartStatus = "Active",
                    CustomerId = userId,
                    RestaurantId = CartList.First().RestaurantId,
                    TotalAmt = CartList.Sum(x => x.TotalPrice),
                    DateAdded = DateTime.Now,
                    TblCartDetails = CartList.Select(c => new TblCartDetail
                    {
                        FoodItemId = c.ItemId,
                        UnitPrice = c.UnitPrice,
                        Quantity = c.Quantity,
                        TotalPrice = c.TotalPrice,
                        DateAdded = DateTime.Now
                    }).ToList()
                };

                _context.TblCartMaster.Add(cartmaster);
                _context.SaveChanges();
            }
            else if (cartMaster != null && CartList.Any())
            {
                if (cartMaster.RestaurantId != CartList.First().RestaurantId)
                    return Ok(new { success = true, conflict = true, userId = userId });

                foreach (var item in CartList)
                {
                    var existingCart = cartMaster.TblCartDetails
                                                 .FirstOrDefault(c => c.FoodItemId == item.ItemId);
                    if (existingCart != null)
                    {
                        existingCart.Quantity += item.Quantity;
                        existingCart.TotalPrice = existingCart.Quantity * existingCart.UnitPrice;
                    }
                    else
                    {
                        cartMaster.TblCartDetails.Add(new TblCartDetail
                        {
                            FoodItemId = item.ItemId,
                            UnitPrice = item.UnitPrice,
                            Quantity = item.Quantity,
                            TotalPrice = item.Quantity * item.UnitPrice,
                            DateAdded = DateTime.Now
                        });
                    }
                }

                cartMaster.TotalAmt = cartMaster.TblCartDetails.Sum(x => x.TotalPrice);
                _context.SaveChanges();
            }

            return Ok(new { success = true, conflict = false, userId = userId });
        }
        
        [HttpGet("getprofile/{userId}")]
        public IActionResult ProfileLoad(int userId)
        {
            if (userId==0)
            {
                return Ok();
            }
            var customer = _context.TblCstmrs.FirstOrDefault(c => c.CustomerId == userId);
            if(customer==null)
            {
                return Ok();
            }
           
            return Ok(new
            {
                
                    customerName = customer.CustomerName,
                    phone = customer.Phone,
                    email = customer.Email,
                    address = customer.Address
                    // ✅ Password, Role — never sent!
                
            });
            
        }

        [HttpPost("userregister")]
        public IActionResult UserRegistration([FromBody]RegisterDto tbl)
        {
            if (ModelState.IsValid)
            {
                var customer = _context.TblCstmrs.FirstOrDefault(c => c.UserName == tbl.UserName);
                if (customer != null)
                {

                    return Ok(new { success = false, message = "User Already exist" });//// stay, keep form values, if redirecttoaction then values will be lost it will treat as new request
                }
                else
                {
                    var customerNew = new TblCstmr
                    {
                        UserName = tbl.UserName,
                        Password =BCrypt.Net.BCrypt.HashPassword(tbl.Password),
                        Address = tbl.Address,
                        Phone = tbl.Phone,
                        Role = "Customer"

                    };
                    _context.TblCstmrs.Add(customerNew);
                    _context.SaveChanges();
                    return Ok(new { success = true, message = "User Registered Successfully",userId=customerNew.CustomerId });
                }
            }
            return BadRequest();
        }
       

    }
}
