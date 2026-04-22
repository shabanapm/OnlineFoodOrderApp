using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging.Abstractions;
using OnlineFoodOrderApp.Data.DTO;
using OnlineFoodOrderApp.Models;
using OnlineFoodOrderApp.ViewModel;
using OnlineFoodOrderApp_API.Data.DTO;
using OnlineFoodOrderApp_API.ViewModel;

namespace OnlineFoodOrderApp.Services
{
    public class CartService
    {
        private readonly HttpClient _httpClient;
        public CartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponseDto> AddToCartAsync(AddToCartRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5034/api/cart/addtocart", request);
             var apiresponse = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
            return apiresponse ?? new ApiResponseDto { Success = false };
        }
        public async Task<CartResponseDto> GetCartAsync(ViewCartRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5034/api/cart/gettocart", request);//Sending a POST request to the API endpoint with request(serialised to json) as the JSON body and waits for the response.
            if (response.IsSuccessStatusCode)
            {
                var apiresponse = await response.Content.ReadFromJsonAsync<CartResponseDto>();//gets response here ViewCartViewModel
                return apiresponse;
            }
            return new CartResponseDto { CartItems=new List<CartItemViewModel>(),RestaurantId=0,RestaurantName="" } ;
        }
        public async Task<Dictionary<int,string>>GetFoodNameAsync(List<int> foodIds)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5034/api/cart/getfoodnames",foodIds);
            if(response.IsSuccessStatusCode)
            {
                var apiresponse = await response.Content.ReadFromJsonAsync<Dictionary<int,string>>();
                return apiresponse;
            }
            return new Dictionary<int, string>();
        }
        public async Task<UpdateCartResponseDto> UpdateCartAsync(UpdateCartDto cartDto)
        {
            var response = await _httpClient.PutAsJsonAsync("http://localhost:5034/api/cart/updatecart", cartDto);
            if(response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadFromJsonAsync<UpdateCartResponseDto>();
                return apiResponse;
            }
            return new UpdateCartResponseDto { Price=0,Quantity=0};
        }
        public async Task<bool>ClearCartAsync(int id)
        {
            var apiResponse = await _httpClient.DeleteAsync("http://localhost:5034/api/cart/clearcart/"+id);
            if(apiResponse.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> ResolvecartAsync(ResolveCartConflictDto resolveCart)
        {
            var apiResponse = await _httpClient.PutAsJsonAsync("http://localhost:5034/api/cart/resolvecart",resolveCart);
            if (apiResponse.IsSuccessStatusCode)
                return true;
            return false;
        }
        
        public async Task<CheckoutViewModel> CheckOutAsync(int id)
        {
            var apiResponse = await _httpClient.GetAsync($"http://localhost:5034/api/cart/checkoutcart/{id}");
            if(apiResponse.IsSuccessStatusCode)
            {
                var apiResult=await apiResponse.Content.ReadFromJsonAsync<CheckoutViewModel>();
                return apiResult ?? new CheckoutViewModel();
            }
            return new CheckoutViewModel();
        }
        public async Task <int> CreateOrderAsync(int id)
        {
            var apiresponse=await _httpClient.PostAsync($"http://localhost:5034/api/order/createorder/{id}",null);
            if (apiresponse.IsSuccessStatusCode)
            {
                var apiResult = await apiresponse.Content.ReadFromJsonAsync<int>();
                return apiResult;
            }
            return  0;
        }
        public async Task<OrderSummaryViewModel> OrderSummaryAsync(int userId,int orderId)
        {
            var apiresponse = await _httpClient.GetAsync($"http://localhost:5034/api/order/getordersummary?userId={userId}&orderId={orderId}");
            var json = await apiresponse.Content.ReadAsStringAsync();
            if (apiresponse.IsSuccessStatusCode)
            {

                var result = await apiresponse.Content.ReadFromJsonAsync<OrderSummaryViewModel>();
                return result;
            }
            return new OrderSummaryViewModel();
        }

        public async Task<List<OrderViewModel>> OrderDisplayAsync(int userId)
        {
            var apiresponse = await _httpClient.GetAsync($"http://localhost:5034/api/order/Orderdisplay?userId={userId}");
            if (apiresponse.IsSuccessStatusCode)
            {
                var apiResult = await apiresponse.Content.ReadFromJsonAsync<List<OrderViewModel>>();
                return apiResult;
            }
            return new List<OrderViewModel>();
        }
        public async Task<MenuViewModel> GetMenuAsync(int restId)
        {
            var apiResponse = await _httpClient.GetAsync($"http://localhost:5034/api/restaurant/getmenu?restId={restId}");
            if(apiResponse.IsSuccessStatusCode)
            {
                var apiResult=await apiResponse.Content.ReadFromJsonAsync<MenuViewModel>();
                return apiResult;
            }
            return new MenuViewModel();
        }
        public async Task<ApiLoginResponseDto> LoginClickAsync(LoginDto loginDto)
        {
            var apiResponse = await _httpClient.PostAsJsonAsync("http://localhost:5034/api/account/loginclick", loginDto);
            if(apiResponse.IsSuccessStatusCode)
            {
                var apiresult=await apiResponse.Content.ReadFromJsonAsync<ApiLoginResponseDto>();
                return apiresult?? new ApiLoginResponseDto();
            }
            return new ApiLoginResponseDto();
        }

        public async Task<CustomerDto> ProfileLoadAsync(int userId)
        {
            try
            {
                var apiresponse = await _httpClient.GetAsync($"http://localhost:5034/api/account/getprofile/{userId}");
                if (apiresponse.IsSuccessStatusCode)
                {
                    var apiResult = await apiresponse.Content.ReadFromJsonAsync<CustomerDto>();
                    return apiResult;
                }
                return new CustomerDto();
            }
            catch (Exception ex)
            {
                return new CustomerDto();

            }
        }
        public async Task<List<RestaurantDto>> LoadRestaurantAsync()
        {
            try
            {
                var apiResponse = await _httpClient.GetAsync("http://localhost:5034/api/restaurant/loadrestaurant");
                if (apiResponse.IsSuccessStatusCode)
                {
                    var apiResult = await apiResponse.Content.ReadFromJsonAsync<List<RestaurantDto>>();
                    return apiResult;
                }
                return new List<RestaurantDto>(); 
            }
           catch (Exception ex)
            {
                return new List<RestaurantDto>();
            }
           
        }
        public async Task<ApiLoginResponseDto> UserRegisterAsyn(RegisterViewModel registerViewModel)
        {
            var apiResponse = await _httpClient.PostAsJsonAsync("http://localhost:5034/api/account/userregister",registerViewModel);
            if (apiResponse.IsSuccessStatusCode)
                {
                var apiResult = await apiResponse.Content.ReadFromJsonAsync<ApiLoginResponseDto>();
                return apiResult;
                }
            return new ApiLoginResponseDto
            {
                success = false,
                message = "Something went wrong. Please try again."
            };
        }
        
    }
}
