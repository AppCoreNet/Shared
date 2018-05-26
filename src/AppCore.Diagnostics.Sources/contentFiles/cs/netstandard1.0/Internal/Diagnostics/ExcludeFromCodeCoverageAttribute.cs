﻿// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis
{
#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6 || NETCOREAPP1_0 || NETCOREAPP1_1

    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Constructor
        | AttributeTargets.Method
        | AttributeTargets.Property
        | AttributeTargets.Event,
        Inherited = false)]
    [ExcludeFromCodeCoverage]
    internal sealed class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }

#endif
}
