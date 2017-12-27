using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SHSecurityContext.Base
{
   public interface IBaseRepository<T> where T : class
    {
        void SaveChanges();
        T Add(T data);
        IEnumerable<T> AddRange(IEnumerable<T> entitys);
        bool Remove(T data);

        bool RemoveRange(IEnumerable<T> entitys);

        bool Update(T data);

        int Max(Expression<Func<T, int>> whereLamdba);

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns></returns>
        bool Exist(Expression<Func<T, bool>> anyLambda);

        /// <summary>
        /// 得到数据集
        /// </summary>
        /// <returns></returns>
        DbSet<T> Set();

        T Find(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">isAsc</param>
        /// <returns></returns>
        IQueryable<T> FindList(Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc);

        /// <summary>
        /// 查找分页数据列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalRecord">总记录数</param>
        /// <param name="whereLamdba">查询表达式</param>
        /// <param name="orderName">排序名称</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        IQueryable<T> FindPageList(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc);
    }
}
