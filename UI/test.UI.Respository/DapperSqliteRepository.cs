using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using sweet.framework.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2015/12/14 14:44:18
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace test.UI.Respository
{
    public class DapperSqliteRepository<TEntity> : IRepository<TEntity>
        where TEntity : class,IEntity, new()
    {
        //连接数据库字符串。
        //private readonly string connectionString = "Data Source=test.db";

        private readonly string _databaseName;

        private readonly DapperExtensionsConfiguration _config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>),
                                                                                                   new List<Assembly>(),
                                                                                                   new SqliteDialect());

        public DapperSqliteRepository(string fileName)
        {
            _databaseName = fileName;
            DapperExtensions.DapperExtensions.Configure(_config);
            //SQLiteConnection.CreateFile(databaseName);
        }

        protected DbConnection OpenConnection()
        {
            string connectionString = "Data Source=" + _databaseName;
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            conn.Open();
            return conn;
        }

        protected IDatabase OpenDatabase()
        {
            var sqlGenerator = new SqlGeneratorImpl(_config);
            var db = new Database(OpenConnection(), sqlGenerator);
            return db;
        }

        public TEntity Insert(TEntity entity)
        {
            using (DbConnection conn = OpenConnection())
            {
                int id = conn.Insert(entity);
                return entity;
            }
        }

        public int InsertTransaction(IEnumerable<TEntity> entityList)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        //public IQueryable<TEntity> LoadEntities(IPredicate predicate)
        //{
        //    using (IDatabase db = OpenDatabase())
        //    {
        //        //var predicate = Predicates.Field<TEntity>(expression, op, value);
        //        IEnumerable<TEntity> list = db.GetList<TEntity>(predicate);

        //        return list.AsQueryable();
        //    }
        //}

        public IQueryable<TEntity> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount,
                                                       Expression<Func<TEntity, bool>> whereLambda,
                                                       Expression<Func<TEntity, S>> orderLambda, bool isAsc = true)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int RemoveTransaction(IEnumerable<TEntity> entityList)
        {
            throw new NotImplementedException();
        }

        public bool SaveOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public int SaveOrUpdateTransaction(IEnumerable<TEntity> entityList)
        {
            throw new NotImplementedException();
        }

        public bool Update(TEntity entity)
        {
            using (IDatabase db = OpenDatabase())
            {
                //TEntity entity = conn.Get<TEntity>(entityId);
                //person.LastName = "Baz";
                return db.Update(entity);
            }
        }

        public int UpdateTransaction(IEnumerable<TEntity> entityList)
        {
            throw new NotImplementedException();
        }
    }
}