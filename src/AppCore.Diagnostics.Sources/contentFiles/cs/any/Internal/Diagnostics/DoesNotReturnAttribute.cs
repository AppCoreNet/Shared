// Licensed under the MIT License.
// Copyright (c) 2022 the AppCore .NET project.

#if (!NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1) || ENABLE_NULLABLE

namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Method)]
    [ExcludeFromCodeCoverage]
    internal sealed class DoesNotReturnAttribute : Attribute
    {
    }
}

#endif