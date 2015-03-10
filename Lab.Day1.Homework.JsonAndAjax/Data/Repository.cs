using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Lab.Day1.Homework.JsonAndAjax.Model;

namespace Lab.Day1.Homework.JsonAndAjax.Data
{
    public class Repository<TEntity> : IDisposable, IRepository<TEntity, int> where TEntity : class
    {
        private readonly DbContext _context;
        private bool _isDisposed = false;

        public Repository(DbContext context = null)
        {
            _context = context ?? new AuthenticationContext();
        }

        #region IRepository implementation
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            List<TEntity> entities;
            try
            {
                entities = _context.Set<TEntity>().Where(predicate).ToList();
            }
            catch (Exception)
            {
                entities = new List<TEntity>(0);
            }

            return entities;
        }

        public List<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            if (entity == null) return;
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_isDisposed) throw new ObjectDisposedException("Current object is already disposed");
                if (_context != null)
                {
                    _context.Dispose();
                }
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Repository()
        {
            Dispose(false);
        }
    }
}