using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace UsersAPI.Controllers
{
    public class AutenticacaoController(IAutenticacaoService autenticacaoService) : Controller
    {

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var retorno = await autenticacaoService.LoginAsync(login.Email, login.Senha);

            return Ok(retorno);
        }
    }
}
