using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AutenticacaoService(IUnitOfWork unitOfWork, IPasswordService passwordService, ITokenService tokenService) : IAutenticacaoService
    {
        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var usuario = await unitOfWork.Users.ObterPorEmailAsync(email);

            if (usuario != null && !string.IsNullOrEmpty(usuario.Nome))
            {
                var senhaValida = passwordService.VerifyPassword(password, usuario.Senha);

                if (senhaValida)
                {
                    // GOTO: Gerar token de autenticação (exemplo simples, use JWT ou similar em produção)
                    var token = tokenService.GenerateAccessToken(usuario, usuario.Role.ToString());

                    return new LoginResponse
                    {
                        Sucesso = true,
                        Token = token,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        Mensagem = "Login bem-sucedido."
                    };
                }
                else
                {
                    return new LoginResponse
                    {
                        Sucesso = false,
                        Token = null,
                        Nome = null,
                        Email = null,
                        Mensagem = "Não autorizado."
                    };
                }
            }
            else
            {
                return new LoginResponse
                {
                    Sucesso = false,
                    Token = null,
                    Nome = null,
                    Email = null,
                    Mensagem = "Login não encontrado."
                };
            }
        }
    }
}
