using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MichalBialecki.com.SF.ServiceBusExample.MessageProcessor
{
    public class ServiceBusCommunicationListener : IServiceBusCommunicationListener
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ServiceBusCommunicationListener(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
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
        }
    }
}
