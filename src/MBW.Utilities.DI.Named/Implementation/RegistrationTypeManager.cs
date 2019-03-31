using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace MBW.Utilities.DI.Named.Implementation
{
    internal static class RegistrationTypeManager
    {
        private const string AssemblyName = "NamedDI.DynamicTypes";

        private static ModuleBuilder _moduleBuilder;
        private static readonly ConcurrentDictionary<(Type serviceType, string name), Type> _types = new ConcurrentDictionary<(Type serviceType, string name), Type>();

        static RegistrationTypeManager()
        {
            AssemblyName an = new AssemblyName(AssemblyName);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        }

        public static Type GetRegistrationType(Type serviceType, string name, bool allowCreate)
        {
            (Type serviceType, string name) key = (serviceType, name);

            if (!allowCreate)
            {
                _types.TryGetValue(key, out Type type);
                return type;
            }

            return _types.GetOrAdd(key, tuple =>
            {
                string typeName = $"{serviceType.FullName}__{name}";

                // Create and record new type
                TypeBuilder tb1 = _moduleBuilder.DefineType(typeName,
                    TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, null);

                return tb1.CreateTypeInfo().AsType();
            });
        }
    }
}