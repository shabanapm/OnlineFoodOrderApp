using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{
    [Table("Tbl_OrderDetail")]
    public class TblOrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int FoodItemId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        public int? Quantity { get; set; }

        [ForeignKey("FoodItemId")]
        [InverseProperty("TblOrderDetails")]
        public virtual TblFoodItem FoodItem { get; set; } = null!;

        [ForeignKey("OrderId")]
        [InverseProperty("TblOrderDetails")]
        public virtual TblOrder Order { get; set; } = null!;
    }
}
