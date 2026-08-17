using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Observability;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository(FCGDbContext dbContext) : IUserRepository
    {
        private const string Entity = "User";

        public Task<User> AdicionarAsync(User entidade) =>
            DatabaseMetrics.TrackAsync(nameof(UserRepository) + "." + nameof(AdicionarAsync), Entity, async () =>
            {
                var retorno = await dbContext.Users.AddAsync(entidade);

                return retorno.Entity;
            });

        public Task Atualizar(User User) =>
            DatabaseMetrics.TrackAsync(nameof(UserRepository) + "." + nameof(Atualizar), Entity, () =>
            {
                dbContext.Users.Update(User);
                return Task.CompletedTask;
            });

        public Task DeletarAsync(User User) =>
            DatabaseMetrics.TrackAsync(nameof(UserRepository) + "." + nameof(DeletarAsync), Entity, () =>
            {
                dbContext.Users.Remove(User);
                return Task.CompletedTask;
            });

        public Task<User> ObterPorEmailAsync(string email) =>
            DatabaseMetrics.TrackAsync(nameof(UserRepository) + "." + nameof(ObterPorEmailAsync), Entity, () =>
                dbContext.Users.FirstOrDefaultAsync(u => u.Email == email));

        public Task<User> ObterPorIdAsync(long id) =>
            DatabaseMetrics.TrackAsync(nameof(UserRepository) + "." + nameof(ObterPorIdAsync), Entity, () =>
                dbContext.Users.FirstOrDefaultAsync(u => u.Id == id));

        public Task<IEnumerable<User>> ObterTodosAsync() =>
            DatabaseMetrics.TrackAsync(nameof(UserRepository) + "." + nameof(ObterTodosAsync), Entity, async () =>
                (IEnumerable<User>)await dbContext.Users.ToListAsync());
    }
}
