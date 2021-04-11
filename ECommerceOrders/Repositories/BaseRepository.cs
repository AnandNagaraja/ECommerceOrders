using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceOrders.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerceOrders.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ReadOnlyECommerceDbContext Context;

        public BaseRepository(ReadOnlyECommerceDbContext context)
        {
            Context = context;
        }

        public async Task<TEntity> GetById(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
    }
}
