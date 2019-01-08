using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace MichalBialecki.com.SF.ServiceBusExample.MessageProcessor
{
    public class ServiceBusStatelessService : StatelessService
    {
        private readonly IServiceBusCommunicationListener _serviceBusCommunicationListener;

        public ServiceBusStatelessService(StatelessServiceContext serviceContext, IServiceBusCommunicationListener serviceBusCommunicationListener)
            : base(serviceContext)
        {
            _serviceBusCommunicationListener = serviceBusCommunicationListener;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return new ServiceInstanceListener(context => _serviceBusCommunicationListener);
        }
    }
}
