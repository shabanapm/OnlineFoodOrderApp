using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{
    [Table("Tbl_CartMaster")]
    public  class TblCartMaster
    {
        [Key]
        public int CartMasterId { get; set; }

        public int CustomerId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateAdded { get; set; }

        public int RestaurantId { get; set; }

        [StringLength(20)]
        public string? CartStatus { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("TblCartMasters")]
        public virtual TblCstmr Customer { get; set; } = null!;

        [ForeignKey("RestaurantId")]
        [InverseProperty("TblCartMasters")]
        public virtual TblRestaurant Restaurant { get; set; } = null!;

        [InverseProperty("CartMaster")]
        public virtual ICollection<TblCartDetail> TblCartDetails { get; set; } = new List<TblCartDetail>();

        [InverseProperty("CartMaster")]
        public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
    }

}
