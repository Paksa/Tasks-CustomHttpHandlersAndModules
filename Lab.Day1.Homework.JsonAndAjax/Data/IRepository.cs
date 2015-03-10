using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Day1.Homework.JsonAndAjax.Data
{
    interface IRepository<TEntity, TKey>
    {
        List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetAll();
        void Update(TEntity entity);
        void Remove(TKey id);
        void Add(TEntity entity);
    }
}
