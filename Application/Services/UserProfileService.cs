using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserProfileService(IUserProfileRepository userProfileRepository, ICacheService cacheService) : IUserProfileService
    {
        private static string ChaveCache(int userId) => $"users:perfil:{userId}";

        public async Task<UserProfile?> ObterAsync(int userId)
        {
            var chave = ChaveCache(userId);

            var emCache = await cacheService.ObterAsync<UserProfile>(chave);
            if (emCache != null)
                return emCache;

            var perfil = await userProfileRepository.ObterPorUsuarioAsync(userId);

            if (perfil != null)
                await cacheService.DefinirAsync(chave, perfil, TimeSpan.FromMinutes(10));

            return perfil;
        }

        public async Task<UserProfile> SalvarAsync(int userId, UserProfileRequest request)
        {
            var perfil = new UserProfile
            {
                UserId = userId,
                Bio = request.Bio,
                AvatarUrl = request.AvatarUrl,
                Preferencias = request.Preferencias,
                AtualizadoEm = DateTime.UtcNow
            };

            var salvo = await userProfileRepository.SalvarAsync(perfil);

            await cacheService.RemoverAsync(ChaveCache(userId));

            return salvo;
        }
    }
}
