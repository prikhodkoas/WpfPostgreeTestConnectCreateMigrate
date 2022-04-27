using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services;

public class RootRepository<T> : IRootRepository<T> where T : BaseEntity
{
    protected readonly ApplicationContext _context;
    private readonly DbSet<T> _entities;

    public RootRepository()
    {
        _context = new ApplicationContext();
        _entities = _context.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return _entities;
        // return _context.Set<T>
    }

    public T GetById(Guid id)
    {
        return _entities.FirstOrDefault(p => p.Id == id);
        //return _context.Set<T>.FirstOrDefault(p => p.Id == id);
    }

    public Guid Insert(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _entities.Add(entity);
        //_context.Set<T>.Add(entity);

        _context.SaveChanges();
        return entity.Id;
    }

    public void Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
    }

    public bool DeleteById(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        var entity = GetById(id);

        if (entity != null)
        {
            try
            {
                _entities.Remove(entity);
                //_context.Set<T>.Remove(entity);

                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
        }

        return true;
    }
}