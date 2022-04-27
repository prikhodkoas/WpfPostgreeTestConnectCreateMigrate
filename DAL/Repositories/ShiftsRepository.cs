using DAL.Entities;

namespace DAL.Services;

public class ShiftsRepository : RootRepository<Shift>, IShiftsRepository
{
    public Shift GetShiftByNumber(int number)
    {
        return _context.Shifts.FirstOrDefault(p => p.Number == number);
    }
}