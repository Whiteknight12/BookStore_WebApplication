using KeBanSach.DataAccess.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KeBanSach.DataAccess.Data.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private ApplicationDbContext _db;
        private DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet=_db.Set<T>();
        }
        public void Delete(T entity)
        {
            if (entity != null) _dbSet.Remove(entity);
        }
        public IEnumerable<T> GetAll()
        {
            IQueryable<T> q = _dbSet;
            return q.ToList();
        }
        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> q= _dbSet;
            q=q.Where(filter);
            return q.FirstOrDefault();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
        public IEnumerable<T> GetList (Expression<Func<T, bool>> filter)
        {
            IQueryable<T> q = _dbSet;
            var selectedlistT=q.Where(filter).ToList();
            return selectedlistT;
        }
    }
}
