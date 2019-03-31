using System;
using MBW.Utilities.DI.Named.Implementation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
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