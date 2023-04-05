using System.Linq.Expressions;

namespace ChatRoom.IRepository
{
    public interface IBaseRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T FirstOrDefault(Expression<Func<T, bool>> filter);
        (IEnumerable<T> data, int total) Get(Expression<Func<T, object>>[]? includeProperties, Expression<Func<T, bool>>? filter, Expression<Func<T, object>>? orderBy, int? pageSize, int? page);
        T GetById(int id, Expression<Func<T, object>>[]? includeProperties);
        IEnumerable<T> GetAll(Expression<Func<T, object>>[]? includeProperties, Expression<Func<T, bool>>? filter, Expression<Func<T, object>>? orderBy);
    }
}
