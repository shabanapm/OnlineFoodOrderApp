using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{

    [Table("Tbl_Transactn")]

    public class TblTransactn
    {
        [Key]
        public int TransactionId { get; set; }

        public int OrderId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("TblTransactns")]
        public virtual TblOrder Order { get; set; } = null!;
    }
}
