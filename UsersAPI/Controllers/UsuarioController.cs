using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
namespace UsersAPI.Controllers
{
    public class UsuarioController(IUserService usuarioService) : Controller
    {

        [HttpPost("usuario/criar")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] UserRequest usuarioRequest)
        {
            try
            {
                var retorno = await usuarioService.CriarUserAsync(usuarioRequest);
                return Created("", retorno);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}\n{ex.StackTrace}\n{ex.InnerException?.Message}");
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("usuario/todos")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var retorno = await usuarioService.ObterTodosAsync();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}\n{ex.StackTrace}\n{ex.InnerException?.Message}");
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var retorno = await usuarioService.ObterPorIdAsync(id);

            if (retorno == null) return NotFound();

            return Ok(retorno);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("usuario/atualizar")]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest usuario)
        {
            await usuarioService.AtualizarAsync(usuario);
            return NoContent();
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("usuario/deletar/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await usuarioService.DeletarAsync(id);
            return NoContent();
        }
    }
}
