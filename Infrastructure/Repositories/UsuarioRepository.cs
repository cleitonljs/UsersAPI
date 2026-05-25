using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
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

        public async Task<User> AdicionarAsync(User entidade)
        {
            var retorno = await dbContext.Users.AddAsync(entidade);

            return retorno.Entity;
        }

        public async Task Atualizar(User User)
        {
            dbContext.Users.Update(User);
        }

        public async Task DeletarAsync(User User)
        {
            dbContext.Users.Remove(User);
        }

        public async Task<User> ObterPorEmailAsync(string email)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> ObterPorIdAsync(long id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> ObterTodosAsync()
        {
            return await dbContext.Users.ToListAsync();
        }
    }
}
