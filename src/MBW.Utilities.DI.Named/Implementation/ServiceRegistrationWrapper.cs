using System;

namespace MBW.Utilities.DI.Named.Implementation
{
    internal class ServiceRegistrationWrapper
    {
        public Func<IServiceProvider, object> Factory { get; }

        public ServiceRegistrationWrapper(Func<IServiceProvider, object> factory)
        {
            Factory = factory;
        }
    }
}