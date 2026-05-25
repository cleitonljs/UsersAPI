using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestaHabbitController : ControllerBase
    {      

        
        [HttpPost]        
        public ActionResult Create(IConfiguration configuration, ITestaHabbitService testaHabbitService,[FromBody] UserDto userDto)
        { 
            try
            {
                testaHabbitService.EnviaMensagem(userDto);
                
                var saida = new {
                    Nome = userDto.Nome,
                    Email = userDto.Email,
                    RabbitMQHost = configuration["RabbitMQ:Host"],
                    RabbitMQPort = configuration["RabbitMQ:Port"],
                    RabbitMQQueue= $"queue:{configuration["RabbitMQ:Queues:FCG"]}",
                    Msg= "Mensagem enviada com sucesso!"
                };
                return Ok(saida);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar mensagem.\n{ex.Message}\n{ex.StackTrace}");
            }
        }        
    }
}
