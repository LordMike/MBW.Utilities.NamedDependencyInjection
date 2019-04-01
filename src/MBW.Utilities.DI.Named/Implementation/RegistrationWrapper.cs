using System;

namespace MBW.Utilities.DI.Named.Implementation
{
    internal class RegistrationWrapper : IDisposable
    {
        public Func<IServiceProvider, object> Factory { private get; set; }

        private bool _created;
        private object _instance;

        public object GetInstance(IServiceProvider serviceProvider)
        {
            if (!_created)
            {
                _instance = Factory(serviceProvider);
                _created = true;
            }

            return _instance;
        }

        public void Dispose()
        {
            (_instance as IDisposable)?.Dispose();
        }
    }
}