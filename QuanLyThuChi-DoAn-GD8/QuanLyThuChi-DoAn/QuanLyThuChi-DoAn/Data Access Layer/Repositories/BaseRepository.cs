using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace QuanLyThuChi_DoAn.Data_Access_Layer.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public virtual T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            if (entityToDelete != null)
            {
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbSet.Remove(entityToDelete);
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}