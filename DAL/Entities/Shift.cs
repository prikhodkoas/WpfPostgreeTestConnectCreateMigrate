using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Shift
    {
        public int Id { get; set; }

        [DisplayName("Номер смены")]
        public int Number { get; set; }

        //[InverseProperty("Shift")]
        public virtual ICollection<CashVoucher> CashVouchers { get; set; }
    }
}
