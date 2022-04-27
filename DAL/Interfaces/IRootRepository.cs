using DAL.Entities;

namespace DAL.Services;

internal interface IRootRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll();
    T GetById(Guid id);
    Guid Insert(T entity);
    void Update(T entity);
    bool DeleteById(Guid id);
}