using Microsoft.EntityFrameworkCore;
using SHSecurityContext.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SHSecurityContext.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected virtual DbContext nContext { get; set; }

        public void SaveChanges()
        {
            nContext.SaveChanges();
        }

        public T Add(T entity)
        {
            nContext.Entry<T>(entity).State = EntityState.Added;
            nContext.SaveChanges();
            return entity;
        }


        public IEnumerable<T> AddRange(IEnumerable<T> entitys)
        {
            foreach (var entity in entitys)
            {
                nContext.Entry<T>(entity).State = EntityState.Added;
            }
            nContext.SaveChanges();
            return entitys;
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return nContext.Set<T>().Count(predicate);
        }

        public bool Update(T entity)
        {
            nContext.Set<T>().Attach(entity);
            nContext.Entry<T>(entity).State = EntityState.Modified;
            return nContext.SaveChanges() > 0;
        }

        public bool Remove(T entity)
        {
            nContext.Set<T>().Attach(entity);
            nContext.Entry<T>(entity).State = EntityState.Deleted;
            return nContext.SaveChanges() > 0;
        }

        public bool RemoveRange(IEnumerable<T> entitys)
        {
            foreach (var entity in entitys)
            {
                nContext.Set<T>().Attach(entity);
                nContext.Entry<T>(entity).State = EntityState.Deleted;
            }
            return nContext.SaveChanges() > 0;
        }


        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return nContext.Set<T>().Any(anyLambda);
        }

        public T Find(Expression<Func<T, bool>> whereLambda)
        {
            T _entity = nContext.Set<T>().FirstOrDefault<T>(whereLambda);
            return _entity;
        }



        public DbSet<T> Set()
        {
            return nContext.Set<T>();
        }

        public IQueryable<T> FindList(Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc)
        {
            var _list = nContext.Set<T>().Where<T>(whereLamdba);
            _list = OrderBy(_list, orderName, isAsc);
            return _list;
        }

        public IQueryable<T> FindPageList(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc)
        {
            var _list = nContext.Set<T>().Where<T>(whereLamdba);
            totalRecord = _list.Count();
            _list = OrderBy(_list, orderName, isAsc).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            return _list;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原IQueryable</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>排序后的IQueryable<T></returns>
        private IQueryable<T> OrderBy(IQueryable<T> source, string propertyName, bool isAsc)
        {
            if (source == null) 
            {
                //throw new ArgumentNullException("source", "不能为空");
                return null;
            }
            if (string.IsNullOrEmpty(propertyName)) return source;
            var _parameter = Expression.Parameter(source.ElementType);
            var _property = Expression.Property(_parameter, propertyName);
            if (_property == null) { 
                // throw new ArgumentNullException("propertyName", "属性不存在");
                  return null;
            }
            var _lambda = Expression.Lambda(_property, _parameter);
            var _methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var _resultExpression = Expression.Call(typeof(Queryable), _methodName, new Type[] { source.ElementType, _property.Type }, source.Expression, Expression.Quote(_lambda));
            return source.Provider.CreateQuery<T>(_resultExpression);
        }

        public int Max(Expression<Func<T, int>> whereLamdba)
        {
            return nContext.Set<T>().Select(whereLamdba).Max();
        }

        public IQueryable<T> FromSql(string sql)
        {
            return nContext.Set<T>().FromSql(sql);
        }

    }
}
