using Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OnlineStore<TEntity> : IOnlineStore<TEntity> where TEntity : class, new()
    {
        protected DBUtils _dBUtils;

        public OnlineStore(DBUtils dBUtils)
        {
            this._dBUtils = dBUtils;
        }

        async Task<TEntity> IOnlineStore<TEntity>.Add(TEntity entity)
        {
            CheckIfNull(entity);
            await _dBUtils.AddAsync(entity);
            await _dBUtils.SaveChangesAsync();
            return entity;
        }

        async Task IOnlineStore<TEntity>.Delete(TEntity entity)
        {
            CheckIfNull(entity);
            _dBUtils.Remove(entity);
            await _dBUtils.SaveChangesAsync();

        }

        IQueryable<TEntity> IOnlineStore<TEntity>.GetAll()
        {
            var data =  _dBUtils.Set<TEntity>();

            if (data == null)
                throw new Exception($"Could not get data of {typeof(TEntity)}");

            return data;
        }

        async Task<TEntity> IOnlineStore<TEntity>.Update(TEntity entity)
        {
            CheckIfNull(entity);
            _dBUtils.Update(entity);
            await _dBUtils.SaveChangesAsync();
            return entity;
        }

        private void CheckIfNull(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException($"{nameof(entity)} shuold not be null.");
        }
    }
}
