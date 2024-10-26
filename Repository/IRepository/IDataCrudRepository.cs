using System.Linq.Expressions;

namespace BookingMovies.Repository.IRepository
{
    public interface IDataCrudRepository<T> where T : class
    {

        IQueryable<T> GetAll();
        void Create(T entity);
        T? GetById(int id);
        //IQueryable<T> GetAllWithInclude(Expression<Func<T, object>> include1, string? include2 = null);
        T? GetOne(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        void Delete(T entity);
        void Commit();

    }
}
