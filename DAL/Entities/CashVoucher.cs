using System.ComponentModel;

namespace DAL.Entities
{
    public class CashVoucher : BaseEntity
    {
        [DisplayName("Номер чека")]
        public int Number { get; set; }

        //[InverseProperty("CashVouchers")]
        public virtual Shift Shift { get; set; }
    }
}