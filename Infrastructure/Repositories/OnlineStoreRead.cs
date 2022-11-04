using Infrastructure.IRepositories;

namespace Infrastructure.Repositories
{
    public class OnlineStoreRead<TEntity> : IOnlineStoreRead<TEntity> where TEntity : class, new()
    {
        protected DBUtils _dBUtils;

        public OnlineStoreRead(DBUtils dBUtils)
        {
            this._dBUtils = dBUtils;
        }

        public IQueryable<TEntity> GetAll()
        {
            var data =  _dBUtils.Set<TEntity>();

            if (data == null)
                throw new Exception($"Could not get data of {typeof(TEntity)}");

            return data;
        }
    }
}
