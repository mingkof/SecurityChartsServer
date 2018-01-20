using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SHSecurityContext.Base
{
    public abstract class BaseService<Model, Context>  where Model : class, new() where Context : DbContext
    {
        public void SaveChanges() { CurrentRepository.SaveChanges(); }

        protected IBaseRepository<Model> CurrentRepository { get; set; }

        public IEnumerable<Model> AddRange(IEnumerable<Model> entityList)
        {
            return CurrentRepository.AddRange(entityList);
        }
        public Model Add(Model entity) { return CurrentRepository.Add(entity); }

        public bool Update(Model entity) { return CurrentRepository.Update(entity); }

        public bool Remove(Model entity) { return CurrentRepository.Remove(entity); }
        public bool RemoveRange(IEnumerable<Model> data)
        {
            return CurrentRepository.RemoveRange(data);
        }

        public int Max(Expression<Func<Model, int>> whereLamdba)
        {
            return CurrentRepository.Max(whereLamdba);
        }
        public int Count(Expression<Func<Model, bool>> predicate)
        {
            return CurrentRepository.Count(predicate);
        }
        public bool Exist(Expression<Func<Model, bool>> anyLambda)
        {
            return CurrentRepository.Exist(anyLambda);
        }

        public Model Find(Expression<Func<Model, bool>> whereLambda)
        {
            return CurrentRepository.Find(whereLambda);
        }

        /*
        public DbSet<Model> Set()
        {
            return CurrentRepository.Set();
        }
        */
        public IQueryable<Model> FindList(Expression<Func<Model, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindList(whereLamdba, orderName, isAsc);
        }

        public IQueryable<Model> FindPageList(int pageIndex, int pageSize, out int totalRecord, Expression<Func<Model, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, whereLamdba, orderName, isAsc);
        }
        public IQueryable<Model> FindPageList(int pageIndex, int pageSize, string seartch, out int totalRecord, Expression<Func<Model, bool>> whereLamdba, string orderName, bool isAsc)
        {
            return CurrentRepository.FindPageList(pageIndex, pageSize, out totalRecord, whereLamdba, orderName, isAsc);
        }


    }
}
