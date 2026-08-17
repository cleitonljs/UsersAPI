using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Observability;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FCGDbContext _dbContext;

        public IUserRepository Users { get; }
        public UnitOfWork(FCGDbContext dbContext)
        {
            _dbContext = dbContext;

            Users = new UserRepository(_dbContext);

        }
        public Task<int> SaveChangesAsync() =>
            DatabaseMetrics.TrackAsync(nameof(UnitOfWork) + "." + nameof(SaveChangesAsync), "*", () =>
                _dbContext.SaveChangesAsync());
    }
}
