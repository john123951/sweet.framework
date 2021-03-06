﻿using LinqToDB;
using LinqToDB.Data;
using sweet.framework.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

/* =======================================================================
* 创建时间：2016/5/17 13:53:23
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace test.UI.Respository
{
    /// <summary>
    /// 使用linq2db访问mysql
    /// https://github.com/linq2db/linq2db
    ///
    /// Supported databases:
    ///     DB2(LUW, z/OS)      Firebird                Informix            Microsoft Access
    ///     Microsoft Sql Azure Microsoft Sql Server    Microsoft SqlCe     MySql
    ///     Oracle              PostgreSQL              SQLite              SAP HANA
    ///     Sybase ASE
    ///
    /// Example:
    ///     <add name="mysql_insurance" connectionString="Server=localhost;Database=insurance;Uid=product;Pwd=shebao$;" providerName="MySql" />
    ///
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Linq2DbRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        public static string DefaultProviderName = ProviderName.MySql;

        private readonly string _providerName;
        private readonly string _connectionString;

        static Linq2DbRepository()
        {
#if DEBUG
            DataConnection.TurnTraceSwitchOn();
            DataConnection.WriteTraceLine = (s, s1) => System.Diagnostics.Debug.WriteLine(s, s1);
#endif
        }

        public Linq2DbRepository(string connectionString)
            : this(null, connectionString)
        {
        }

        public Linq2DbRepository(string providerName, string connectionString)
        {
            _providerName = providerName;
            _connectionString = connectionString;
        }

        protected DataConnection OpenConnection()
        {
            //var conn = LinqToDB.DataProvider.MySql.MySqlTools.CreateDataConnection(_connectionString);
            var conn = _providerName == null ? new DataConnection(DefaultProviderName, _connectionString) : new DataConnection(_providerName, _connectionString);

            return conn;
        }

        public bool Insert(TEntity entity)
        {
            using (var db = OpenConnection())
            {
                int result = db.Insert<TEntity>(entity);
                return result > 0;
            }
        }

        public bool Update(TEntity entity)
        {
            using (var db = OpenConnection())
            {
                int result = db.Update<TEntity>(entity);

                return result > 0;
            }
        }

        public bool SaveOrUpdate(TEntity entity)
        {
            using (var db = OpenConnection())
            {
                int result = db.InsertOrReplace<TEntity>(entity);
                return result > 0;
            }
        }

        public bool Remove(TEntity entity)
        {
            using (var db = OpenConnection())
            {
                int result = db.Delete<TEntity>(entity);

                return result > 0;
            }
        }

        public int InsertTransaction(IEnumerable<TEntity> entityList)
        {
            //using (var transaction = db.BeginTransaction())
            using (var db = OpenConnection())
            using (var transaction = new TransactionScope())
            {
                var result = db.BulkCopy<TEntity>(entityList);

                transaction.Complete();
                return (int)result.RowsCopied;
            }
        }

        public int UpdateTransaction(IEnumerable<TEntity> entityList)
        {
            using (var db = OpenConnection())
            using (var transaction = new TransactionScope())
            {
                foreach (var entity in entityList)
                {
                    int result = db.Update<TEntity>(entity);
                }

                transaction.Complete();
                return entityList.Count();
            }
        }

        public int SaveOrUpdateTransaction(IEnumerable<TEntity> entityList)
        {
            using (var db = OpenConnection())
            using (var transaction = new TransactionScope())
            {
                foreach (var entity in entityList)
                {
                    int result = db.InsertOrReplace<TEntity>(entity);
                }

                transaction.Complete();
                return entityList.Count();
            }
        }

        public int RemoveTransaction(IEnumerable<TEntity> entityList)
        {
            if (entityList == null || entityList.Count() <= 0) { return 0; }

            using (var db = OpenConnection())
            using (var transaction = new TransactionScope())
            {
                foreach (var entity in entityList)
                {
                    int result = db.Delete<TEntity>(entity);

                    if (result <= 0)
                    {
                        transaction.Dispose();
                        return 0;
                    }
                }

                transaction.Complete();

                return entityList.Count();
            }
        }

        public IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLambda)
        {
            using (var db = OpenConnection())
            {
                var list = db.GetTable<TEntity>().Where(whereLambda).ToList().AsQueryable();

                return list;
            }
        }

        public IQueryable<TEntity> LoadPageEntities<TOrder>(int pageIndex, int pageSize, out int totalCount,
                                                       Expression<Func<TEntity, bool>> whereLambda,
                                                       Expression<Func<TEntity, TOrder>> orderLambda, bool isAsc = true)
        {
            using (var db = OpenConnection())
            {
                IQueryable<TEntity> linq = db.GetTable<TEntity>().Where(whereLambda);

                totalCount = linq.Count();

                linq = isAsc ? linq.OrderBy(orderLambda) : linq.OrderByDescending(orderLambda);
                linq = linq.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

                return linq.ToList().AsQueryable();
            }
        }
    }
}