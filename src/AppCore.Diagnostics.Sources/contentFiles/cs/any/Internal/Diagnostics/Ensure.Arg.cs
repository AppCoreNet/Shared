// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace AppCore.Diagnostics
{
    internal static partial class Ensure
    {
        /// <summary>
        /// Provides helper methods to ensure method argument contracts.
        /// </summary>
        #if !APPCORE_SHARED_TEST_SOURCES
        [ExcludeFromCodeCoverage]
        #endif
        internal static partial class Arg
        {
            private static bool HasInterfaceThatMapsToGenericTypeDefinition(Type givenType, Type genericType)
            {
                return givenType
                       .GetTypeInfo()
                       .ImplementedInterfaces
                       .Where(it => it.GetTypeInfo().IsGenericType)
                       .Any(it => it.GetGenericTypeDefinition() == genericType);
            }

            private static bool MapsToGenericTypeDefinition(Type givenType, Type genericType)
            {
                return genericType.GetTypeInfo().IsGenericTypeDefinition
                       && givenType.GetTypeInfo().IsGenericType
                       && givenType.GetGenericTypeDefinition() == genericType;
            }
        }
    }
}
