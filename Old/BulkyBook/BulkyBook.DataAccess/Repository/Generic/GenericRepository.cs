using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        private DbSet<T> dbSet;
        public GenericRepository(ApplicationDBContext db)
        {
            _db = db;
            //_db.ShoppingCart.AsNoTracking()
            //_db.ShoppingCart.Include(x => x.prod).Include(x => x.CoverType);
            dbSet = _db.Set<T>();
        }

        void IGenericRepository<T>.Add(T entity)
        {
            dbSet.Add(entity);
        }
        //includeProp - "Category,CoverType"
        IEnumerable<T> IGenericRepository<T>.GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (includeProperties is not null)
                query = IncludeProperties(includeProperties);

            if(filter is not null)
                query = query.Where(filter);

			return query.ToList();
        }

        T IGenericRepository<T>.GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool isTracked = true)
        {
            IQueryable<T> query;

            //by default it tracks the entity
			//if tracked is false. EF Core will not tracked the set.
			//if you save the changes in the object. dbset is set as no tracking it will not save.
			//you will need to do _unitOfWork.object.Update(object); in order to update the changes.
			if (isTracked)
                query = dbSet;
            else
                query = dbSet.AsNoTracking();
            
            if (includeProperties is not null)
                query = IncludeProperties(includeProperties);

            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        void IGenericRepository<T>.Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        void IGenericRepository<T>.RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        private IQueryable<T> IncludeProperties(string? includeProperties)
        {
            IQueryable<T> query = dbSet;

            foreach(var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

            return query;
        }
    }
}
