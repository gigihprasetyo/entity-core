using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace qcs_product.EventBus.EventBus.GooglePubSub
{
    public class GooglePubSubClient
       : IGooglePubSubClient
    {
        private readonly ILogger<GooglePubSubClient> _logger;
        string _projectId;
        string _topicId;
        string _subscriptionId;

        public GooglePubSubClient(ILogger<GooglePubSubClient> logger,
            string projectId,
            string topicId,
            string subscriptionId = "")
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _projectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
            _topicId = topicId ?? throw new ArgumentNullException(nameof(topicId));
            _subscriptionId = subscriptionId ?? throw new ArgumentNullException(nameof(subscriptionId));
        }

        public PublisherClient GetPublisher()
        {
            var topicName = TopicName.FromProjectTopic(_projectId, _topicId);
            _logger.LogTrace("Creating publisher client {projectId} - {topicId}", _projectId, _topicId);
            return PublisherClient.Create(topicName);
        }

        public SubscriberClient GetSubscriber()
        {
            SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(_projectId, _subscriptionId);
            return SubscriberClient.Create(subscriptionName);
        }
    }
}
