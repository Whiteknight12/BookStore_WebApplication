using System.Linq.Expressions;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public void Delete(T entity);
        public IEnumerable<T> GetAll();
        public T Get(Expression<Func<T, bool>> filter);
        public void Save();
        public IEnumerable<T> GetList(Expression<Func<T, bool>> filter);
    }
}
