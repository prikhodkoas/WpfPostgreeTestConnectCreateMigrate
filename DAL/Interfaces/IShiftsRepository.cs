using DAL.Entities;

namespace DAL.Services;

public interface IShiftsRepository
{
    IQueryable<Shift> GetAll();
    Shift GetById(Guid id);
    Guid Insert(Shift entity);
    void Update(Shift entity);
    bool DeleteById(Guid id);

    Shift GetShiftByNumber(int number);
}