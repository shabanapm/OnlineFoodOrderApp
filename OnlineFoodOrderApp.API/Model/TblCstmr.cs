using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Model
{
    [Table("Tbl_Cstmr")]
    public class TblCstmr
    {

        [Key]
        public int CustomerId { get; set; }

        [StringLength(50)]
        public string? CustomerName { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? UserName { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }

        [StringLength(20)]
        public string? Role { get; set; }

        [InverseProperty("Customer")]
        public virtual ICollection<TblCartMaster> TblCartMasters { get; set; } = new List<TblCartMaster>();

        //[InverseProperty("Customer")]
        //public virtual ICollection<TblDeliveryAddr> TblDeliveryAddrs { get; set; } = new List<TblDeliveryAddr>();

        [InverseProperty("Customer")]
        public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
    }
}
