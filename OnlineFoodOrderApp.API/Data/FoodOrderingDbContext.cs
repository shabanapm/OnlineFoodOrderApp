using Microsoft.EntityFrameworkCore;
using FoodOrderingAPI.Model;
namespace FoodOrderingAPI.Data
{
    public class FoodOrderingDbContext:DbContext
    {
        public FoodOrderingDbContext(DbContextOptions<FoodOrderingDbContext> options)
           : base(options) { }
        public DbSet<TblCartMaster> TblCartMaster { get; set; }
        public DbSet<TblCartDetail> TblCartDetails { get; set; }
        public DbSet<TblCstmr> TblCstmrs { get; set; }
        public DbSet<TblRestaurant> TblRestaurants { get; set; }
        public DbSet<TblFoodItem> TblFoodItems { get; set; }

        public DbSet<TblOrder> TblOrders { get; set; }

        public DbSet<TblTransactn>TblTransactns { get; set; }
        public DbSet<TblOrderDetail> TblOrderDetails { get; set; }
        public DbSet<TblRestaurantFudItem> TblRestaurantFudItems { get; set; }


    }
}
