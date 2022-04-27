using DAL.Entities;

namespace DAL.Services;

public interface ICashVouchersRepository
{
    IQueryable<CashVoucher> GetAll();
    CashVoucher GetById(int id);
    int Insert(CashVoucher entity);
    void Update(CashVoucher entity);
    bool DeleteById(int id);
}