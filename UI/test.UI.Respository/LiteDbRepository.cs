using LiteDB;
using sweet.framework.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace test.UI.Respository
{
    public class LiteDbRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly string _conn;
        private object _lock = new object();

        public LiteDbRepository(string fileName)
        {
            _conn = fileName;
        }

        public TEntity Insert(TEntity entity)
        {
            lock (_lock)
            {
                using (var db = new LiteDatabase(_conn))
                {
                    //获取 customers 集合，如果没有会创建，相当于表
                    var collection = db.GetCollection<TEntity>(typeof(TEntity).Name);

                    // 将新的对象插入到数据表中，Id是自增，自动生成的
                    collection.Insert(entity);

                    return entity;
                }
            }
        }

        public bool Update(TEntity entity)
        {
            using (var db = new LiteDatabase(_conn))
            {
                var collection = db.GetCollection<TEntity>(typeof(TEntity).Name);

                //保存到数据库
                return collection.Update(entity);
            }
        }

        public bool SaveOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int InsertTransaction(IEnumerable<TEntity> entityList)
        {
            throw new NotImplementedException();
        }

        public int UpdateTransaction(IEnumerable<TEntity> entityList)
        {
            throw new NotImplementedException();
        }

        public int SaveOrUpdateTransaction(IEnumerable<TEntity> entityList)
        {
            throw new NotImplementedException();
        }

        public int RemoveTransaction(IEnumerable<TEntity> entityList)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLambda)
        {
            using (var db = new LiteDatabase(_conn))
            {
                var collection = db.GetCollection<TEntity>(typeof(TEntity).Name);

                return collection.Find(whereLambda).ToList().AsQueryable();
            }
        }

        public IQueryable<TEntity> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount,
                                                       Expression<Func<TEntity, bool>> whereLambda,
                                                       Expression<Func<TEntity, S>> orderLambda, bool isAsc = true)
        {
            using (var db = new LiteDatabase(_conn))
            {
                var collection = db.GetCollection<TEntity>(typeof(TEntity).Name);
                int skip = pageSize * (pageIndex - 1);

                IQueryable<TEntity> linq = collection.Find(whereLambda, skip, pageSize).ToList().AsQueryable();

                totalCount = collection.Count(whereLambda);

                return linq;
            }
        }
    }
}