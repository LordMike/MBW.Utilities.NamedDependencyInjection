﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MBW.Utilities.DI.Named.Implementation
{
    internal static class RegistrationTypeManager
    {
        public const string AssemblyName = "NamedDI.DynamicTypes";
        public const string MainModuleName = "MainModule";
        private static readonly ModuleBuilder _moduleBuilder;

        /// <summary>
        /// Unique type+name => marker-type registrations
        /// </summary>
        private static readonly ConcurrentDictionary<(Type serviceType, string name), Type> _registrationTypes = new();

        /// <summary>
        /// Map service types to names, for listing purposes
        /// </summary>
        private static readonly ConcurrentDictionary<Type, (string name, Type registrationType)[]> _registrationTypesByServiceType = new();

        static RegistrationTypeManager()
        {
            AssemblyName an = new AssemblyName(AssemblyName);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule(MainModuleName);
        }

        public static Type GetRegistrationWrapperType(Type serviceType, string name, bool allowCreate)
        {
            (Type serviceType, string name) key = (serviceType, name);

            if (!allowCreate)
            {
                _registrationTypes.TryGetValue(key, out Type type);
                return type;
            }

            return _registrationTypes.GetOrAdd(key, tuple =>
            {
                string typeName = $"{serviceType.FullName}__{name}";

                // Create and record new type
                TypeBuilder typeBuilder = _moduleBuilder.DefineType(typeName,
                    TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout, null);

                typeBuilder.SetParent(typeof(RegistrationWrapper));

                Type registrationWrapperType = typeBuilder.CreateTypeInfo().AsType();

                _registrationTypesByServiceType.AddOrUpdate(tuple.serviceType,
                    svcType =>
                    {
                        (string name, Type registrationType)[] newList = new (string name, Type registrationType)[1];
                        newList[0] = (tuple.name, registrationWrapperType);

                        return newList;
                    },
                    (type, existingList) =>
                    {
                        // Note: We create new lists here to be able to return immutable lists when queried
                        (string name, Type registrationType)[] newList = new (string name, Type registrationType)[existingList.Length + 1];
                        existingList.CopyTo(newList, 0);

                        newList[newList.Length - 1] = (tuple.name, registrationWrapperType);

                        return newList;
                    });

                return registrationWrapperType;
            });
        }

        public static IEnumerable<(string name, Type registrationType)> GetRegistrationTypesAndNames(Type serviceType)
        {
            if (!_registrationTypesByServiceType.TryGetValue(serviceType, out var lst))
                return Enumerable.Empty<(string, Type)>();

            return lst;
        }
    }
}