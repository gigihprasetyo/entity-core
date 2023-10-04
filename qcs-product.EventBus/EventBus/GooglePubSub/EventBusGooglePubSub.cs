using Autofac;
using qcs_product.EventBus.EventBus.Base;
using qcs_product.EventBus.EventBus.Base.Abstractions;
using qcs_product.EventBus.EventBus.Base.Events;
using qcs_product.EventBus.EventBus.Base.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using System.Threading;
using static Google.Cloud.PubSub.V1.SubscriberClient;
using System.Collections.Generic;
using qcs_product.EventBus.EventbUS.Constants;

namespace qcs_product.EventBus.EventBus.GooglePubSub
{
    public class EventBusGooglePubSub : IEventBus
    {
        const string AUTOFAC_SCOPE_NAME = "q100_platform";
        private readonly ILogger<EventBusGooglePubSub> _logger;
        private readonly IGooglePubSubClient _pubSubClient;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;

        public EventBusGooglePubSub(
            ILogger<EventBusGooglePubSub> logger,
            IGooglePubSubClient pubSubClient,
            IEventBusSubscriptionsManager subsManager,
            ILifetimeScope autofac)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pubSubClient = pubSubClient ?? throw new ArgumentNullException(nameof(pubSubClient));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _autofac = autofac;
        }

        public async Task PublishAsync(IntegrationEvent @event)
        {
            PublisherClient publisher = _pubSubClient.GetPublisher();
            string eventName = @event.GetType().Name;

            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var messageInstance = new PubsubMessage()
            {
                Data = ByteString.CopyFrom(body)
            };
            messageInstance.Attributes.Add(Q100EventBusConstant.PUBSUB_EVENTNAME_ATTRIBUTE, eventName);

            try
            {
                string published = await publisher.PublishAsync(messageInstance);
                _logger.LogTrace("Publishing event to GooglePubSub: {desc}", published);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not publish event: {EventId} ({ExceptionMessage})", @event.Id, ex.Message);
                throw ex;
            }
        }

        public void StartSubscribing()
        {
            var subscriberClient = _pubSubClient.GetSubscriber();

            subscriberClient.StartAsync(Consumer_Received);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());

            _subsManager.AddSubscription<T, TH>();
        }

        private async Task<Reply> Consumer_Received(PubsubMessage msg, CancellationToken cancelToken)
        {
            string message = msg.Data.ToStringUtf8();
            string eventName = msg.Attributes.GetValueOrDefault(Q100EventBusConstant.PUBSUB_EVENTNAME_ATTRIBUTE);

            Reply result = Reply.Ack;

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
                result = Reply.Nack;
            }

            return result;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing PubSub event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod(Q100EventBusConstant.MESSAGE_HANDLER_METHOD_NAME).Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
            else
            {
                _logger.LogInformation("No subscription for PubSub event: {EventName}", eventName);
            }
        }
    }
}
