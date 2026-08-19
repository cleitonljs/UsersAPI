using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService, IUserCreatedProducer userCreatedProducer, ICacheService cacheService) : IUserService
    {
        private const string ChaveListaTodos = "users:todos";
        private static string ChaveUsuario(int id) => $"users:usuario:{id}";

        public bool ValidaEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.com$");
            return regex.IsMatch(email);
        }

        public bool ValidaSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) || senha.Length < 8)
                return false;

            var hasLetter = senha.Any(char.IsLetter);
            var hasDigit = senha.Any(char.IsDigit);
            var hasSpecial = senha.Any(ch => !char.IsLetterOrDigit(ch));

            return hasLetter && hasDigit && hasSpecial;
        }

        public async Task<User> CriarUserAsync(UserRequest User)
        {
            if (!ValidaEmail(User.Email))
                throw new ArgumentException("Email inválido.");
            if (!ValidaSenha(User.Senha))
                throw new ArgumentException("Senha deve conter pelo menos 8 caracteres, incluindo letras, números e caracteres especiais.");

            var usu = mapper.Map<User>(User);

            usu.Senha = passwordService.HashPassword(User.Senha);
            usu.Role = 2;

            var UserCriado = await unitOfWork.Users.AdicionarAsync(usu);

            await unitOfWork.SaveChangesAsync();

            await cacheService.RemoverAsync(ChaveListaTodos);

            var evento = new UserCreatedEvent()
            {
                Nome = UserCriado.Nome,
                Email = UserCriado.Email
            };

            await userCreatedProducer.UserCreatedSend(evento);

            return UserCriado;
        }

        public async Task<IEnumerable<User>> ObterTodosAsync()
        {
            var emCache = await cacheService.ObterAsync<List<User>>(ChaveListaTodos);
            if (emCache != null)
                return emCache;

            var usuarios = await unitOfWork.Users.ObterTodosAsync();
            var lista = usuarios.ToList();

            await cacheService.DefinirAsync(ChaveListaTodos, lista, TimeSpan.FromSeconds(60));

            return lista;
        }

        public async Task<User> ObterPorIdAsync(int id)
        {
            var chave = ChaveUsuario(id);

            var emCache = await cacheService.ObterAsync<User>(chave);
            if (emCache != null)
                return emCache;

            var usuario = await unitOfWork.Users.ObterPorIdAsync(id);

            if (usuario != null)
                await cacheService.DefinirAsync(chave, usuario, TimeSpan.FromSeconds(60));

            return usuario;
        }

        public async Task AtualizarAsync(UserUpdateRequest entidade)
        {
            var User = await unitOfWork.Users.ObterPorIdAsync(entidade.Id) ?? throw new DirectoryNotFoundException(entidade.Id.ToString());

            User.Nome = entidade.Nome;
            User.Email = entidade.Email;

            await unitOfWork.Users.Atualizar(User);

            await unitOfWork.SaveChangesAsync();

            await cacheService.RemoverAsync(ChaveListaTodos);
            await cacheService.RemoverAsync(ChaveUsuario(entidade.Id));
        }

        public async Task DeletarAsync(int id)
        {
            var User = await unitOfWork.Users.ObterPorIdAsync(id) ?? throw new DirectoryNotFoundException(id.ToString());

            await unitOfWork.Users.DeletarAsync(User);

            await unitOfWork.SaveChangesAsync();

            await cacheService.RemoverAsync(ChaveListaTodos);
            await cacheService.RemoverAsync(ChaveUsuario(id));
        }
    }
}
