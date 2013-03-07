﻿using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using RabbitMQ.Client;

namespace SignalR.RabbitMQ
{
    public static class DependencyResolverExtensions
    {
		public static IDependencyResolver UseRabbitMq(this IDependencyResolver resolver, string ampqConnectionString, string exchangeName, string queueName = null)
        {
            if (string.IsNullOrEmpty(exchangeName))
            {
                throw new ArgumentNullException("exchangeName");
            }

            if (string.IsNullOrEmpty(ampqConnectionString))
            {
                throw new ArgumentNullException("ampqConnectionString");
            }

            var bus = new Lazy<RabbitMqMessageBus>(() => new RabbitMqMessageBus(resolver, ampqConnectionString, exchangeName, queueName));

            resolver.Register(typeof(IMessageBus), () => bus.Value);

            return resolver;
        }

		public static IDependencyResolver UseRabbitMq(this IDependencyResolver resolver, ConnectionFactory connectionfactory, string exchangeName, string queueName = null)
        {
            if (string.IsNullOrEmpty(exchangeName))
            {
                throw new ArgumentNullException("exchangeName");
            }

            if (connectionfactory == null)
            {
                throw new ArgumentNullException("connectionfactory");
            }

            var ampqConnectionString = string.Format("host={0};virtualHost={1};username={2};password={3};requestedHeartbeat=10", connectionfactory.HostName, connectionfactory.VirtualHost, connectionfactory.UserName, connectionfactory.Password);
            var bus = new Lazy<RabbitMqMessageBus>(() => new RabbitMqMessageBus(resolver, ampqConnectionString, exchangeName, queueName));

            resolver.Register(typeof(IMessageBus), () => bus.Value);

            return resolver;
        }
    }
}
