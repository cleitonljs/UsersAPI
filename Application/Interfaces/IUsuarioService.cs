using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CriarUserAsync(UserRequest User);
        Task<IEnumerable<User>> ObterTodosAsync();
        Task<User> ObterPorIdAsync(int id);
        Task AtualizarAsync(UserUpdateRequest User);
        Task DeletarAsync(int id);
    }
}
