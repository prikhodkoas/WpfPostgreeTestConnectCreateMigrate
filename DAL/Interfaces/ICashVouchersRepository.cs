using DAL.Entities;

namespace DAL.Services;

public interface ICashVouchersRepository
{
    IQueryable<CashVoucher> GetAll();
    CashVoucher GetById(Guid id);
    Guid Insert(CashVoucher entity);
    void Update(CashVoucher entity);
    bool DeleteById(Guid id);
}