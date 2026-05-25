using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> AdicionarAsync(User entidade);
        Task<User> ObterPorEmailAsync(string email);
        Task<User> ObterPorIdAsync(long id);
        Task Atualizar(User User);
        Task DeletarAsync(User User);

    }
}
