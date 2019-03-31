using System;
using Microsoft.Extensions.DependencyInjection;
using RF.Tracking.Libraries.NamedDI.Implementation;

namespace RF.Tracking.Libraries.NamedDI.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider provider, string name)
        {
            NamedServiceFactory factory = provider.GetRequiredService<NamedServiceFactory>();
            object service = factory.Create(typeof(T), name);

            if (service == null)
                return default;

            return (T)service;
        }

        public static T GetRequiredService<T>(this IServiceProvider provider, string name)
        {
            NamedServiceFactory factory = provider.GetRequiredService<NamedServiceFactory>();
            object service = factory.Create(typeof(T), name);

            if (service == null)
                throw new Exception($"Service of type {typeof(T).FullName} not registered with name {name}");

            return (T)service;
        }
    }
}