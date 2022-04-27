using DAL.Entities;

namespace DAL.Services;

public interface IShiftsRepository
{
    IQueryable<Shift> GetAll();
    Shift GetById(int id);
    int Insert(Shift entity);
    void Update(Shift entity);
    bool DeleteById(int id);

    Shift GetShiftByNumber(int number);
}