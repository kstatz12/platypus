using System;
using System.Collections.Generic;
using System.Linq;
using Platypus.Command;
using Platypus.Event;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Platypus.Bus.RabbitMq
{
    public class RabbitMqBus : IBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly List<Tuple<string, Func<IModel, EventingBasicConsumer>>> _handlers;
        public RabbitMqBus(IBusHost host)
        {
            var factory = new ConnectionFactory
            {
                HostName = host.Host,
                UserName = host.UserName,
                Password = host.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _handlers = new List<Tuple<string, Func<IModel, EventingBasicConsumer>>>();
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }

        public void Start()
        {
            _handlers.ForEach(x =>
            {
                if (_connection.IsOpen)
                {
                    _channel.BasicConsume(x.Item1, false, x.Item2(_channel));
                }
            });
        }

        public void Publish<T>(T @event, Dictionary<string, object> args = null) where T : IEvent
        {
            if (args == null)
            {
                BasicEmit(@event, _channel);
            }
            else
            {
                AdvancedEmit(args, @event, _channel);
            }
        }

        public void Send<T>(T command, Dictionary<string, object> args = null) where T : ICommand
        {
            if (args == null)
            {
                BasicEmit(command, _channel);
            }
            else
            {
                AdvancedEmit(args, command, _channel);
            }
        }

        public void RegisterHandler<T>(Action<T> action, Dictionary<string, object> args = null)
        {
            if (args == null)
            {
                BasicConsume(action);
            }
            else
            {
                AdvancedConsume(action, args);
            }
        }

        private void AdvancedEmit<T>(Dictionary<string, object> args, T message, IModel channel)
        {
            var exchange = ConfigureExchange(args, channel);
            var routingKey = ConfigureRoutingKey(args);
            var body = message.Convert();
            var properties = GetBasicProperties();
            channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: properties, body: body);
        }

        private void BasicEmit<T>(T message, IModel channel)
        {
            var queueName = typeof(T).ToQueue().CreateQueue(channel);
            var body = message.Convert();
            var properties = GetBasicProperties();
            if (_connection.IsOpen)
            {
                _channel.BasicPublish("", queueName, false, properties, body);
            }
        }

        private void AdvancedConsume<T>(Action<T> action, Dictionary<string, object> args)
        {
            var queueName = GetAdvancedQueueName(args, typeof(T));
            Func<IModel, EventingBasicConsumer> consumer = (channel) =>
            {
                queueName.CreateQueue(channel);
                var exchange = ConfigureExchange(args, channel);
                var routingKey = ConfigureRoutingKey(args);
                BindQueue(queueName, exchange, routingKey, channel);
                var basicConsumer = new EventingBasicConsumer(channel);
                var prefetch = GetPrefetch(args);
                InitQos(channel, prefetch);
                basicConsumer.Received += (sender, arguments) =>
                {
                    var body = arguments.Body.Convert<T>();
                    action(body);
                    channel.BasicAck(arguments.DeliveryTag, false);
                };
                return basicConsumer;
            };
            _handlers.Add(new Tuple<string, Func<IModel, EventingBasicConsumer>>(queueName, consumer));
        }

        private void BasicConsume<T>(Action<T> action)
        {
            var queueName = typeof(T).ToQueue();
            Func<IModel, EventingBasicConsumer> consumer = (channel) =>
            {
                queueName.CreateQueue(channel);
                var basicConsumer = new EventingBasicConsumer(channel);
                InitQos(channel);
                basicConsumer.Received += (sender, args) => {
                    var body = args.Body.Convert<T>();
                    action(body);
                    channel.BasicAck(args.DeliveryTag, false);
                };
                return basicConsumer;
            };
            _handlers.Add(new Tuple<string, Func<IModel, EventingBasicConsumer>>(queueName, consumer));
        }

        private static string ConfigureExchange(Dictionary<string, object> args, IModel channel)
        {
            var exchangeName = args.Where(x => x.Key.ToLower().Equals("exchangename")).Select(x => x.Value).FirstOrDefault();
            var exchangeType = args.Where(x => x.Key.ToLower().Equals("exchangetype")).Select(x => x.Value).FirstOrDefault();

            if (exchangeName != null && exchangeType != null)
                channel.ExchangeDeclare(exchangeName.ToString(), exchangeType.ToString(), durable: true);

            return exchangeName?.ToString() ?? "";
        }

        private static string ConfigureRoutingKey(Dictionary<string, object> args)
        {
            var routingKey = args.Where(x => x.Key.ToLower().Equals("routingkey")).Select(x => x.Value).FirstOrDefault();
            return routingKey?.ToString() ?? "";
        }

        private IBasicProperties GetBasicProperties()
        {
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            return properties;
        }

        private void BindQueue(string queue, string exchange, string routingKey, IModel channel)
        {
            channel.QueueBind(queue, exchange, routingKey);
        }

        private static ushort GetPrefetch(Dictionary<string, object> args)
        {
            var prefetchRaw = args.Where(x => x.Key.ToLower().Equals("prefetch"))
                                       .Select(x => x.Value)
                                       .FirstOrDefault();
            if (!ushort.TryParse(prefetchRaw?.ToString(), out ushort prefetch))
            {
                prefetch = 1;
            }
            return prefetch;
        }

        private static string GetAdvancedQueueName(Dictionary<string, object> args, Type t)
        {
            var queue =
                args.Where(x => x.Key.ToLower().Equals("queuename")).Select(x => x.Value).FirstOrDefault();
            return queue?.ToString() ?? t.ToQueue();
        }

        private static void InitQos(IModel channel, ushort prefetch = 1, bool global = false)
        {
            channel.BasicQos(0, prefetch, global);
        }
    }
}