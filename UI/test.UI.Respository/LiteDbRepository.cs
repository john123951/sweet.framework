using sweet.framework.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace test.UI.Respository
{
    //public class Linq2DbRepository<TEntity> : IRepository<TEntity>
    //     where TEntity : class, IEntity, new()
    //{
    //    private readonly string _connectionString;

    //    public Linq2DbRepository(string connectionString)
    //    {
    //        _connectionString = connectionString;
    //    }

    //    protected LinqToDB.Data.DataConnection OpenConnection()
    //    {
    //        var conn = MySqlTools.CreateDataConnection(_connectionString);

    //        return conn;
    //    }

    //    public bool Insert(TEntity entity)
    //    {
    //        using (var db = OpenConnection())
    //        {
    //            int result = db.Insert<TEntity>(entity);

    //            return result > 0;
    //        }
    //    }

    //    public bool InsertTransaction(IEnumerable<TEntity> entityList)
    //    {
    //        using (var db = OpenConnection())
    //        {
    //            db.BulkCopy<TEntity>(entityList);

    //            return true;
    //        }
    //    }

    //    public IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLambda)
    //    {
    //        using (var db = OpenConnection())
    //        {
    //            var list = db.GetTable<TEntity>().Where(whereLambda)
    //                                             .ToList()
    //                                             .AsQueryable();

    //            return list;
    //        }
    //    }

    //    public IQueryable<TEntity> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount,
    //                                                   Expression<Func<TEntity, bool>> whereLambda,
    //                                                   Expression<Func<TEntity, object>> orderLambda, bool isAsc = true)
    //    {
    //        using (var db = OpenConnection())
    //        {
    //            IQueryable<TEntity> linq = db.GetTable<TEntity>().Where(whereLambda);

    //            totalCount = linq.Count();

    //            linq = isAsc ? linq.OrderBy(orderLambda) : linq.OrderByDescending(orderLambda);
    //            linq = linq.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

    //            return linq;
    //        }
    //    }

    //    public bool Remove(TEntity entity)
    //    {
    //        using (var db = OpenConnection())
    //        {
    //            int result = db.Delete<TEntity>(entity);

    //            if (result > 0)
    //            {
    //                return true;
    //            }

    //            return false;
    //        }
    //    }

    //    public bool RemoveTransaction(IEnumerable<TEntity> entityList)
    //    {
    //        using (var db = OpenConnection())
    //        using (var transaction = new TransactionScope())
    //        {
    //            foreach (var entity in entityList)
    //            {
    //                int result = db.Delete<TEntity>(entity);

    //                if (result <= 0)
    //                {
    //                    db.RollbackTransaction();
    //                    return false;
    //                }
    //            }
    //            transaction.Complete();

    //            return true;
    //        }
    //    }

    //    public bool SaveOrUpdate(TEntity entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public int SaveOrUpdateTransaction(IEnumerable<TEntity> entityList)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Update(TEntity entity)
    //    {
    //        using (var db = OpenConnection())
    //        {
    //            int result = db.Update<TEntity>(entity);

    //            return result > 0;
    //        }
    //    }

    //    public int UpdateTransaction(IEnumerable<TEntity> entityList)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}