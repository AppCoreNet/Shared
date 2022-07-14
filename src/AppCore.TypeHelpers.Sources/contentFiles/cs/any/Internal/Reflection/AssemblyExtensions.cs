// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1 || ENABLE_NULLABLE
    #nullable enable
#endif

namespace AppCore.Reflection
{
    #if !APPCORE_SHARED_TEST_SOURCES
    [ExcludeFromCodeCoverage]
    #endif
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Gets all types defined in the given <paramref name="assembly"/> which assignable from the specified <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        /// This methods supports open generics.
        /// </remarks>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesAssignableFrom(this Assembly assembly, Type type)
        {
            Ensure.Arg.NotNull(assembly, nameof(assembly));
            Ensure.Arg.NotNull(type, nameof(type));

            return assembly.GetTypes()
                           .Where(
                               t => t.GetTypesAssignableFrom(true)
                                     .Contains(type));
        }

        /// <summary>
        /// Gets all exported types defined in the given <paramref name="assembly"/> which assignable from the specified <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        /// This methods supports open generics.
        /// </remarks>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetExportedTypesAssignableFrom(this Assembly assembly, Type type)
        {
            Ensure.Arg.NotNull(assembly, nameof(assembly));
            Ensure.Arg.NotNull(type, nameof(type));

            return assembly.GetExportedTypes()
                           .Where(
                               t => t.GetTypesAssignableFrom(true)
                                     .Contains(type));
        }
    }
}