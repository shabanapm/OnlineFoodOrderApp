using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{
    [Table("Tbl_Order")]
    public class TblOrder
    {
        [Key]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OrderAmt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }

        public int? RestaurantId { get; set; }

        public int? CartMasterId { get; set; }

        [StringLength(50)]
        public string? OrderStatus { get; set; }

        [ForeignKey("CartMasterId")]
        [InverseProperty("TblOrders")]
        public virtual TblCartMaster? CartMaster { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("TblOrders")]
        public virtual TblCstmr Customer { get; set; } = null!;

        [ForeignKey("RestaurantId")]
        [InverseProperty("TblOrders")]
        public virtual TblRestaurant? Restaurant { get; set; }

        [InverseProperty("Order")]
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; } = new List<TblOrderDetail>();

        [InverseProperty("Order")]
        public virtual ICollection<TblTransactn> TblTransactns { get; set; } = new List<TblTransactn>();
    }
}
