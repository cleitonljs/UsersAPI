using Application.DTOs;
using Application.Interfaces;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Producers
{
    public class UserCreatedProducer : IUserCreatedProducer
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IConfiguration _cfg;

        public UserCreatedProducer(ISendEndpointProvider sendEndpointProvider, IConfiguration cfg) 
        {
            _sendEndpointProvider = sendEndpointProvider;
            _cfg = cfg;
        }

        public async Task UserCreatedSend(UserDto user)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(
                    new Uri($"queue:{_cfg["RabbitMQ:Queues:FCG"]}"));

            await endpoint.Send(user);
        }        
    }
}
