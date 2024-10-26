using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookingMovies.Repository
{
    public class DataCrudRepository<T> : IDataCrudRepository<T> where T:class
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> dbSet;

        public DataCrudRepository(ApplicationDbContext context) {
            this.context = context;
         dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll() 
        {
            return dbSet;
        }

        //public IQueryable<T> GetAllWithInclude(Expression<Func<T, object>> include1 =nu)
        //{
            
        //        IQueryable<T> query = dbSet.Include(include1);
        //    if (include2 != null)
        //    {
        //        query= dbSet.Include(include1).ThenInclude(include2);
        //        return query;
        //    }
        //    return query;
        //}

        public void Create(T entity)
        {
          dbSet.Add(entity);
        }

        public T? GetById(int id) 
        {

            var result= dbSet.Find(id);
            if (result == null) 
                return null;
            else 
                return result;
        }

        public T? GetOne(Expression<Func< T,bool>> predicate)
        {

            var result = dbSet.Where(predicate).FirstOrDefault();
            if (result == null)
                return null;
            else
                return result;
        }
        public void Update(T entity)
        { 
                dbSet.Update(entity);
        }

        public void Delete(T entity) {
          
                dbSet.Remove( entity);
        }

        public void Commit()
        {
            context.SaveChanges();
        }


    }
}
