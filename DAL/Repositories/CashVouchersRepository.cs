using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services;

public class CashVouchersRepository : RootRepository<CashVoucher>, ICashVouchersRepository
{
    //public IQueryable<CashVoucher> GetAll()
    //{
    //    return _context.CashVouchers.Include(p => p.Shift);
    //}

    public Guid Insert(CashVoucher cashVoucher)
    {
        if (cashVoucher == null)
        {
            throw new ArgumentNullException(nameof(cashVoucher));
        }

        if (cashVoucher.Shift == null)
        {
            throw new ArgumentNullException(nameof(cashVoucher.Shift));
        }

        cashVoucher.Shift = _context.Shifts.FirstOrDefault(p => p.Id == cashVoucher.Shift.Id);

        _context.CashVouchers.Add(cashVoucher);
        _context.SaveChanges();

        return cashVoucher.Id;
    }
}