// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;

namespace AppCore.Reflection
{
    [ExcludeFromCodeCoverage]
    internal static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetExportedClosedTypesOf(this Assembly assembly, Type openGeneric)
        {
            Ensure.Arg.NotNull(assembly, nameof(assembly));
            Ensure.Arg.NotNull(openGeneric, nameof(openGeneric));

            return assembly.ExportedTypes
                           .Where(t => t.IsClosedTypeOf(openGeneric));
        }
    }
}