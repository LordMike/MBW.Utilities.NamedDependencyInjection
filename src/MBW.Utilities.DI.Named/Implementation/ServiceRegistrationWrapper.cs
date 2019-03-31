using System;

namespace RF.Tracking.Libraries.NamedDI.Implementation
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