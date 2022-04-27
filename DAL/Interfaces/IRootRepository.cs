using DAL.Entities;

namespace DAL.Services;

internal interface IRootRepository<T> where T : BaseEntity
{
    public IQueryable<T> GetAll();
    public T GetById(int id);
    public int Insert(T entity);
    public void Update(T entity);
    public bool DeleteById(int id);
}