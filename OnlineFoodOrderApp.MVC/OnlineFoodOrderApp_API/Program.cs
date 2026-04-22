using Microsoft.EntityFrameworkCore;
using OnlineFoodOrderApp.Data;
using OnlineFoodOrderApp.Services;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<FoodOrderDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpClient<CartService>();



var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Restaurant}/{action=Load_Restaurants}/{id?}");

app.Run();
