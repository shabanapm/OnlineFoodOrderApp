using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{
    [Table("Tbl_RestaurantFudItem")]
    public class TblRestaurantFudItem
    {
        [Key]
        public int RestaurantFoodItemId { get; set; }

        public int RestaurantId { get; set; }

        public int FoodItemId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal FoodPrice { get; set; }

        [ForeignKey("FoodItemId")]
        [InverseProperty("TblRestaurantFudItems")]
        public virtual TblFoodItem FoodItem { get; set; } = null!;

        [ForeignKey("RestaurantId")]
        [InverseProperty("TblRestaurantFudItems")]
        public virtual TblRestaurant Restaurant { get; set; } = null!;
    }
}
