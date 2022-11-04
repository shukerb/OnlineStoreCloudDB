using Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OnlineStoreWrite<TEntity> : IOnlineStoreWrite<TEntity> where TEntity : class, new()
    {
        protected DBUtils _dBUtils;

        public OnlineStoreWrite(DBUtils dBUtils)
        {
            this._dBUtils = dBUtils;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            CheckIfNull(entity);
            await _dBUtils.AddAsync(entity);
            await _dBUtils.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(TEntity entity)
        {
            CheckIfNull(entity);
            _dBUtils.Remove(entity);
            await _dBUtils.SaveChangesAsync();

        }
        public async Task<TEntity> Update(TEntity entity)
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
