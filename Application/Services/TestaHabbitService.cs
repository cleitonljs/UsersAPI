using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TestaHabbitService(IUserCreatedProducer userCreatedProducer) : ITestaHabbitService
    {
        public void EnviaMensagem(UserCreatedEvent userDto)
        {
            userCreatedProducer.UserCreatedSend(userDto);
        }
    }
}
