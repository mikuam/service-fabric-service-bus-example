using System.Fabric;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MichalBialecki.com.SF.ServiceBusExample.MessageProcessor
{
    public static class ContainerConfig
    {
        private static ServiceProvider ServiceProvider;

        public static void Init(
            StatelessServiceContext context,
            IConfigurationRoot configuration)
        {
            ServiceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(context)
                .AddSingleton<ServiceBusStatelessService>()
                .AddSingleton<IServiceBusCommunicationListener, ServiceBusCommunicationListener>()
                .AddSingleton<IConfigurationRoot>(configuration)
                .BuildServiceProvider();
        }

        public static TService GetInstance<TService>() where TService : class
        {
            return ServiceProvider.GetService<TService>();
        }
    }
}
