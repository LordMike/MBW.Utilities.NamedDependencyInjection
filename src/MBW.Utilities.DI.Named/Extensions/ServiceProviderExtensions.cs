using System;
using MBW.Utilities.DI.Named.Implementation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider provider, string name)
        {
            Type registrationType = RegistrationTypeManager.GetRegistrationType(typeof(T), name, false);
            if (registrationType == null)
                return default;

            return (T)provider.GetService(registrationType);
        }

        public static T GetRequiredService<T>(this IServiceProvider provider, string name)
        {
            Type registrationType = RegistrationTypeManager.GetRegistrationType(typeof(T), name, false);
            if (registrationType == null)
                throw new Exception($"Service of type {typeof(T).FullName} not registered with name {name}");

            return (T)provider.GetRequiredService(registrationType);
        }
    }
}