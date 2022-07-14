// Licensed under the MIT License.
// Copyright (c) 2022 the AppCore .NET project.

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1

namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(
        AttributeTargets.Field
        | AttributeTargets.Parameter
        | AttributeTargets.Property
        | AttributeTargets.ReturnValue)]
    [ExcludeFromCodeCoverage]
    internal sealed class NotNullAttribute : Attribute
    {
    }
}

#endif