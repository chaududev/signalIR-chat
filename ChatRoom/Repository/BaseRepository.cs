using ChatRoom.Data;
using ChatRoom.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace ChatRoom.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;

        public BaseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Add(T entity)
        {
            db.Add(entity);
            db.SaveChanges();

        }

        public void Delete(T entity)
        {
            db.Set<T>().Remove(entity);
            db.SaveChanges();

        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return db.Set<T>().FirstOrDefault(filter);
        }

        public (IEnumerable<T> data, int total) Get(Expression<Func<T, object>>[]? includeProperties, Expression<Func<T, bool>>? filter, Expression<Func<T, object>>? orderBy, int? pageSize, int? page)
        {
            IQueryable<T> rs = db.Set<T>();
            if (filter != null)
                rs = rs.Where(filter);
            var total = rs.Count();
            if (orderBy != null)
            {
                rs = rs.OrderBy(orderBy);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    rs = rs.Include(includeProperty);
                }
            }
            int pageNumber = page ?? 1;
            int pageSizes = pageSize ?? 100;
            rs = rs.Skip((pageNumber - 1) * pageSizes).Take(pageSizes);
            return (rs.ToList(), total);

        }

        public IEnumerable<T> GetAll(Expression<Func<T, object>>[]? includeProperties,Expression<Func<T, bool>>? filter, Expression<Func<T, object>>? orderBy)
        {
            IQueryable<T> rs = db.Set<T>();
            if (filter != null)
                rs = rs.Where(filter);
            if (orderBy != null)
            {
                rs = rs.OrderByDescending(orderBy);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    rs = rs.Include(includeProperty);
                }
            }
            return rs.ToList();
        }

        public T GetById(int id, Expression<Func<T, object>>[]? includeProperties)
        {
            IQueryable<T> rs = db.Set<T>();
            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    rs = rs.Include(property);
                }
            }
            T entity = rs.FirstOrDefault(x => EF.Property<int>(x, "Id") == id);
            return entity;
        }

        public void Update(T entity)
        {

            db.Set<T>().Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();

        }
    }
}
