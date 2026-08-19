using Application.DTOs;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile?> ObterAsync(int userId);
        Task<UserProfile> SalvarAsync(int userId, UserProfileRequest request);
    }
}
