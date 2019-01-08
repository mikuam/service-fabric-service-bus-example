using System;
using System.Diagnostics;
using System.Fabric;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.ServiceFabric.Services.Runtime;

namespace MichalBialecki.com.SF.ServiceBusExample.MessageProcessor
{
    public class Program
    {
        private const string ServiceName = "MichalBialecki.com.SF.ServiceBusExample.MessageProcessorType";

        private static IConfigurationRoot Configuration;

        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            try
            {
                await ServiceRuntime.RegisterServiceAsync(ServiceName, CreateService);

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(MessageProcessor).Name);
                
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static ServiceBusStatelessService CreateService(StatelessServiceContext context)
        {
            ContainerConfig.Init(context, Configuration);
            return ContainerConfig.GetInstance<ServiceBusStatelessService>();
        }
    }
}
