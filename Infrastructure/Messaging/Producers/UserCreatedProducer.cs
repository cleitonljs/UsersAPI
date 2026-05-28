using Application.DTOs;
using Application.Interfaces;
using Domain.Events;
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
    public class UserCreatedProducer(ISendEndpointProvider sendEndpointProvider, IConfiguration cfg) : IUserCreatedProducer
    {
        public async Task UserCreatedSend(UserCreatedEvent user)
        {
            var endpoint = await sendEndpointProvider.GetSendEndpoint(
                    new Uri($"queue:{cfg["RabbitMQ:Queues:FCG_User"]}"));

            await endpoint.Send(user);
        }        
    }
}
