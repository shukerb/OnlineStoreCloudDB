using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepositories
{
    public interface IOnlineStoreRead<TEntity> where TEntity : class,new()
    {
        IQueryable<TEntity> GetAll();
    }
}
