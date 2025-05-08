using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        private readonly ConcurrentDictionary<string, object> _Repositories;

        public UnitOfWork(StoreContext storeContext)
        {
            _storeContext = storeContext;
            _Repositories = new();
        }
        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            //var TypeName = typeof(TEntity).Name;
            //if (_Repositories.ContainsKey(TypeName)) return (IGenericRepository<TEntity, Tkey>)_Repositories[TypeName];
            //var Repository = new GenericRepository<TEntity, Tkey>(_storeContext);
            //_Repositories.GetOrAdd(TypeName, Repository);
            //return Repository;


            return (IGenericRepository<TEntity, Tkey>)_Repositories.GetOrAdd(typeof(TEntity).Name, _ => new GenericRepository<TEntity, Tkey>(_storeContext));
        }

        public async Task<int> SaveChangesAsync()
        => await _storeContext.SaveChangesAsync();
    }
}
