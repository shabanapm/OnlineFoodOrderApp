using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{
    [Table("Tbl_CartDetail")]
    public  class TblCartDetail
    {
        [Key]
        public int CartDetailId { get; set; }

        public int CartMasterId { get; set; }

        public int FoodItemId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateAdded { get; set; }

        [ForeignKey("CartMasterId")]
        [InverseProperty("TblCartDetails")]
        public virtual TblCartMaster CartMaster { get; set; } = null!;

        [ForeignKey("FoodItemId")]
        [InverseProperty("TblCartDetails")]
        public virtual TblFoodItem FoodItem { get; set; } = null!;
    }

}
