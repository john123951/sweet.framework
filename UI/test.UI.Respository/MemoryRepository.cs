using sweet.framework.Infrastructure.Interfaces;
using sweet.framework.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2016/1/23 16:08:56
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace test.UI.Respository
{
    public class MemoryRepository<TEntity> : IRepository<TEntity>
        where TEntity : PrimaryEntity, new()
    {
        private Dictionary<string, Dictionary<long, TEntity>> _database = new Dictionary<string, Dictionary<long, TEntity>>();

        private Dictionary<long, TEntity> GetTable()
        {
            string tableName = typeof(TEntity).Name;

            if (!_database.ContainsKey(tableName))
            {
                _database[tableName] = new Dictionary<long, TEntity>();
            }

            return _database[tableName];
        }

        public bool Insert(TEntity entity)
        {
            var table = GetTable();

            table.Add(entity.Id, entity);

            return true;
        }

        public bool Update(TEntity entity)
        {
            var table = GetTable();

            if (!table.Values.Any(x => x.Id.ToString() == entity.Id.ToString()))
            {
                throw new KeyNotFoundException("key=" + entity.Id);
            }
            table[entity.Id] = entity;

            return true;
        }

        public bool SaveOrUpdate(TEntity entity)
        {
            var table = GetTable();
            table[entity.Id] = entity;

            return true;
        }

        public bool Remove(TEntity entity)
        {
            var table = GetTable();

            return table.Remove(entity.Id);
        }

        public int RemoveTransaction(IEnumerable<TEntity> entityList)
        {
            var table = GetTable();

            foreach (var entity in entityList)
            {
                table.Remove(entity.Id);
            }

            return entityList.Count();
        }

        public int SaveOrUpdateTransaction(IEnumerable<TEntity> entityList)
        {
            var table = GetTable();

            foreach (var entity in entityList)
            {
                if (!table.Values.Any(x => x.Id.ToString() == entity.Id.ToString()))
                {
                    throw new KeyNotFoundException("key=" + entity.Id);
                }
                table[entity.Id] = entity;
            }
            return entityList.Count();
        }

        public int InsertTransaction(IEnumerable<TEntity> entityList)
        {
            var table = GetTable();

            foreach (var entity in entityList)
            {
                table.Add(entity.Id, entity);
            }
            return entityList.Count();
        }

        public int UpdateTransaction(IEnumerable<TEntity> entityList)
        {
            var table = GetTable();

            foreach (var entity in entityList)
            {
                table[entity.Id] = entity;
            }
            return entityList.Count();
        }

        public IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLambda)
        {
            var table = GetTable();
            return table.Values.AsQueryable().Where(whereLambda);
        }

        public IQueryable<TEntity> LoadPageEntities<TOrder>(int pageIndex, int pageSize, out int totalCount, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TOrder>> orderLambda, bool isAsc = true)
        {
            var table = GetTable();

            IQueryable<TEntity> linq = table.Values.AsQueryable().Where(whereLambda);

            totalCount = linq.Count();

            linq = isAsc ? linq.OrderBy(orderLambda) : linq.OrderByDescending(orderLambda);
            linq = linq.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            return linq;
        }
    }
}