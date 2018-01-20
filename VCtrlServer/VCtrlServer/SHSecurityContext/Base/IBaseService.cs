using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SHSecurityContext.Base
{
    public interface IBaseService<T> where T : class, new()
    {
        void SaveChanges();
        T Add(T data);
        IEnumerable<T> AddRange(IEnumerable<T> data);
        bool Remove(T data);

        bool RemoveRange(IEnumerable<T> data);

        bool Update(T data);
        int Max(Expression<Func<T, int>> whereLamdba);
        int Count(Expression<Func<T, bool>> whereLambda);
        bool Exist(Expression<Func<T, bool>> anyLambda);
        // DbSet<T> Set();
        T Find(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> FindList(Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc);
        IQueryable<T> FindPageList(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc);

    }
}
