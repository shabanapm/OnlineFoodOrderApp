using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{
    [Table("Tbl_FoodItem")]
    public class TblFoodItem
    {
        [Key]
        public int FoodItemId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public int FoodCategoryId { get; set; }

        [StringLength(150)]
        public string? ImagePath { get; set; }

        //[ForeignKey("FoodCategoryId")]
        //[InverseProperty("TblFoodItems")]
        //public virtual TblFoodCategory FoodCategory { get; set; } = null!;

        [InverseProperty("FoodItem")]
        public virtual ICollection<TblCartDetail> TblCartDetails { get; set; } = new List<TblCartDetail>();

        [InverseProperty("FoodItem")]
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; } = new List<TblOrderDetail>();

        [InverseProperty("FoodItem")]
        public virtual ICollection<TblRestaurantFudItem> TblRestaurantFudItems { get; set; } = new List<TblRestaurantFudItem>();
    }
}
