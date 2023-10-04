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
    public interface IGooglePubSubClient
    {
        PublisherClient GetPublisher();
        SubscriberClient GetSubscriber();
    }
}
