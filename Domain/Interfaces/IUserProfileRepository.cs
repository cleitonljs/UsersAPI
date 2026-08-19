using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> ObterPorUsuarioAsync(int userId);
        Task<UserProfile> SalvarAsync(UserProfile perfil);
    }
}
