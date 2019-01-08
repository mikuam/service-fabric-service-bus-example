using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MichalBialecki.com.SF.ServiceBusExample.MessageProcessor
{
    public class ServiceBusCommunicationListener : IServiceBusCommunicationListener
    {
        private readonly IConfigurationRoot _configurationRoot;
        private readonly ILogger _logger;

        private SubscriptionClient subscriptionClient;
        
        public ServiceBusCommunicationListener(IConfigurationRoot configurationRoot, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(ServiceBusCommunicationListener));
            _configurationRoot = configurationRoot;
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var sbConnectionString = _configurationRoot.GetConnectionString("ServiceBusConnectionString");
            var topicName = _configurationRoot.GetValue<string>("Settings:TopicName");
            var subscriptionName = _configurationRoot.GetValue<string>("Settings:SubscriptionName");

            subscriptionClient = new SubscriptionClient(sbConnectionString, topicName, subscriptionName);
            subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var messageJson = Encoding.UTF8.GetString(message.Body);
                    // process here

                    Console.WriteLine($"Received message: {messageJson}");

                    await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                },
                new MessageHandlerOptions(LogException)
                    { MaxConcurrentCalls = 1, AutoComplete = false });

            return Task.FromResult(string.Empty);
        }
        
        public Task CloseAsync(CancellationToken cancellationToken)
        {
            Stop();

            return Task.CompletedTask;
        }

        public void Abort()
        {
            Stop();
        }

        private void Stop()
        {
            subscriptionClient?.CloseAsync().GetAwaiter().GetResult();
        }

        private Task LogException(ExceptionReceivedEventArgs args)
        {
            _logger.LogError(args.Exception, args.Exception.Message);

            return Task.CompletedTask;
        }
    }
}
