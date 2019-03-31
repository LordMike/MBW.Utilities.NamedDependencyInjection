using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RF.Tracking.Libraries.NamedDI.Implementation;

namespace RF.Tracking.Libraries.NamedDI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection AddNamedServices(this IServiceCollection services)
        {
            services.TryAddTransient<NamedServiceFactory>();
            return services;
        }

        private static IServiceCollection AddNamed(this IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
        {
            Type registrationType = RegistrationTypeManager.GetRegistrationType(serviceType, name, true);

            ServiceRegistrationWrapper wrapper = new ServiceRegistrationWrapper(factory);

            services
                .AddNamedServices()
                .Add(new ServiceDescriptor(registrationType, ctx => wrapper.Factory.Invoke(ctx), lifetime));

            return services;
        }

        private static IServiceCollection AddNamed(this IServiceCollection services, string name, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            Type registrationType = RegistrationTypeManager.GetRegistrationType(serviceType, name, true);

            services
                .AddNamedServices()
                .Add(new ServiceDescriptor(registrationType, implementationType, lifetime));

            return services;
        }

        public static IServiceCollection AddSingleton<T>(this IServiceCollection services, string name)
        {
            Type type = typeof(T);
            return AddNamed(services, name, type, type, ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddSingleton<T>(this IServiceCollection services, string name, T instance)
        {
            return AddNamed(services, name, typeof(T), ctx => instance, ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddSingleton<T>(this IServiceCollection services, string name, Func<IServiceProvider, T> factory)
        {
            return AddNamed(services, name, typeof(T), provider => factory(provider), ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddTransient<T>(this IServiceCollection services, string name)
        {
            Type type = typeof(T);
            return AddNamed(services, name, type, type, ServiceLifetime.Transient);
        }

        public static IServiceCollection AddTransient<T>(this IServiceCollection services, string name, T instance)
        {
            return AddNamed(services, name, typeof(T), ctx => instance, ServiceLifetime.Transient);
        }

        public static IServiceCollection AddTransient<T>(this IServiceCollection services, string name, Func<IServiceProvider, T> factory)
        {
            return AddNamed(services, name, typeof(T), provider => factory(provider), ServiceLifetime.Transient);
        }

        public static IServiceCollection AddScoped<T>(this IServiceCollection services, string name)
        {
            Type type = typeof(T);
            return AddNamed(services, name, type, type, ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddScoped<T>(this IServiceCollection services, string name, T instance)
        {
            return AddNamed(services, name, typeof(T), ctx => instance, ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddScoped<T>(this IServiceCollection services, string name, Func<IServiceProvider, T> factory)
        {
            return AddNamed(services, name, typeof(T), provider => factory(provider), ServiceLifetime.Scoped);
        }
    }
}