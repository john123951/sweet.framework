﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace sweet.framework.Infrastructure.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        bool Insert(TEntity entity);

        bool Update(TEntity entity);

        bool SaveOrUpdate(TEntity entity);

        bool Remove(TEntity entity);

        int InsertTransaction(IEnumerable<TEntity> entityList);

        int UpdateTransaction(IEnumerable<TEntity> entityList);

        int SaveOrUpdateTransaction(IEnumerable<TEntity> entityList);

        int RemoveTransaction(IEnumerable<TEntity> entityList);

        IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLambda);

        IQueryable<TEntity> LoadPageEntities<TOrder>(int pageIndex, int pageSize, out int totalCount,
                                        Expression<Func<TEntity, bool>> whereLambda,
                                        Expression<Func<TEntity, TOrder>> orderLambda, bool isAsc = true);
    }
}