using Domain.Common.Settings;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Nosql;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IMongoCollection<UserProfile> _profiles;

        public UserProfileRepository(MongoDbContext mongoDbContext, IOptions<MongoSettings> settings)
        {
            _profiles = mongoDbContext.GetCollection<UserProfile>(settings.Value.ProfilesCollection);
        }

        public async Task<UserProfile?> ObterPorUsuarioAsync(int userId)
        {
            var filtro = Builders<UserProfile>.Filter.Eq(p => p.UserId, userId);

            return await _profiles.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<UserProfile> SalvarAsync(UserProfile perfil)
        {
            if (string.IsNullOrWhiteSpace(perfil.Id))
                perfil.Id = ObjectId.GenerateNewId().ToString();

            var filtro = Builders<UserProfile>.Filter.Eq(p => p.UserId, perfil.UserId);

            // Upsert: se já existe perfil pra esse usuário, substitui; senão, cria.
            await _profiles.ReplaceOneAsync(
                filtro,
                perfil,
                new ReplaceOptions { IsUpsert = true });

            return perfil;
        }
    }
}
