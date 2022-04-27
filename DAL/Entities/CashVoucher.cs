using System.ComponentModel;

namespace DAL.Entities
{
    public class CashVoucher : BaseEntity
    {
        [DisplayName("Номер чека")]
        public int Number { get; set; }

        [DisplayName("ID смены")]
        public int? ShiftId { get; set; }

        //[InverseProperty("CashVouchers")]
        public virtual Shift Shift { get; set; }
    }
}