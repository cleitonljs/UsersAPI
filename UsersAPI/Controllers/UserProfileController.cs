using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UsersAPI.Controllers
{
    public class UserProfileController(IUserProfileService userProfileService) : Controller
    {
        // Só o próprio usuário (comparando com o claim do JWT) ou um Administrador
        // podem ver/editar o perfil estendido de um usuário.
        private bool PodeAcessar(int id)
        {
            if (User.IsInRole("Administrador"))
                return true;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out var userId) && userId == id;
        }

        [Authorize]
        [HttpGet("usuario/{id}/perfil")]
        public async Task<IActionResult> Get(int id)
        {
            if (!PodeAcessar(id))
                return Forbid();

            var perfil = await userProfileService.ObterAsync(id);

            if (perfil == null)
                return NotFound();

            return Ok(perfil);
        }

        [Authorize]
        [HttpPut("usuario/{id}/perfil")]
        public async Task<IActionResult> Salvar(int id, [FromBody] UserProfileRequest request)
        {
            if (!PodeAcessar(id))
                return Forbid();

            var perfil = await userProfileService.SalvarAsync(id, request);
            return Ok(perfil);
        }
    }
}
