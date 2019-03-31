using System;

namespace RF.Tracking.Libraries.NamedDI.Implementation
{
    internal class NamedServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public NamedServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object Create(Type type, string name)
        {
            Type registrationType = RegistrationTypeManager.GetRegistrationType(type, name, false);
            if (registrationType == null)
                //throw new InvalidOperationException($"No service for type {type} named '{name}' has been registered");
                return null;

            return _serviceProvider.GetService(registrationType);
        }
    }
}