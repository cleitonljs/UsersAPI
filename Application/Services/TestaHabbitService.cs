using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TestaHabbitService(IUserCreatedProducer userCreatedProducer) : ITestaHabbitService
    {
        public void EnviaMensagem(UserDto userDto)
        {
            userCreatedProducer.UserCreatedSend(userDto);
        }
    }
}
