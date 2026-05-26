using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
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
                var _event = new UserCreatedEvent()
                {
                    Nome = userDto.Nome,
                    Email = userDto.Email
                };

                //testaHabbitService.EnviaMensagem(userDto);
                testaHabbitService.EnviaMensagem(_event);
                
                var saida = new {
                    Nome = userDto.Nome,
                    Email = userDto.Email,
                    RabbitMQHost = configuration["RabbitMQ:Host"],
                    RabbitMQPort = configuration["RabbitMQ:Port"],
                    RabbitMQQueue= $"queue:{configuration["RabbitMQ:FCG_User"]}",
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
