using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{
    [Table("Tbl_Restaurant")]
    public  class TblRestaurant
    {
        [Key]
        public int RestaurantId { get; set; }

        [StringLength(100)]
        public string RestaurantName { get; set; } = null!;

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(300)]
        public string? GoogleLocation { get; set; }

        [StringLength(50)]
        public string? FoodType { get; set; }

        public short? Rating { get; set; }

        [InverseProperty("Restaurant")]
        public virtual ICollection<TblCartMaster> TblCartMasters { get; set; } = new List<TblCartMaster>();

        [InverseProperty("Restaurant")]
        public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

        [InverseProperty("Restaurant")]
        public virtual ICollection<TblRestaurantFudItem> TblRestaurantFudItems { get; set; } = new List<TblRestaurantFudItem>();
    }
}
